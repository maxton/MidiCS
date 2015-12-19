/*
 * MidiMessage.cs
 * 
 * Copyright (c) 2015, maxton. All rights reserved.
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.IO;
using System.Text;
using MidiCS.Events;

namespace MidiCS
{
  public abstract class MidiMessage
  {
    static byte lastStatus = 0;

    /// <summary>
    /// Reads a single Midi message from the given stream.
    /// If the message uses running status, the status from the last call to
    /// this method will be used.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static MidiMessage ReadFrom(Stream s)
    {
      int deltaTime = s.ReadMidiMultiByte();
      byte status = s.ReadUInt8();
      if(status < 0x80) // running status
      {
        status = lastStatus;
        s.Position--;
      }
      else
      {
        if(status < 0xF0) // meta events do not trigger running status?
          lastStatus = status;
      }
      byte channel = (byte)(status & 0xF);
      byte key, velocity, pressure, controller, value;
      ushort pitchBend;
      EventType eventType = (EventType)(status & 0xF0);
      switch(eventType)
      {
        case EventType.NoteOff:
          key = s.ReadUInt8();
          velocity = s.ReadUInt8();
          return new NoteOffEvent(deltaTime, channel, key, velocity);
        case EventType.NoteOn:
          key = s.ReadUInt8();
          velocity = s.ReadUInt8();
          return new NoteOnEvent(deltaTime, channel, key, velocity);
        case EventType.NotePresure:
          key = s.ReadUInt8();
          pressure = s.ReadUInt8();
          return new NotePressureEvent(deltaTime, channel, key, pressure);
        case EventType.Controller:
          controller = s.ReadUInt8();
          value = s.ReadUInt8();
          return new ControllerEvent(deltaTime, channel, controller, value);
        case EventType.ProgramChange:
          value = s.ReadUInt8();
          return new ProgramChgEvent(deltaTime, channel, value);
        case EventType.ChannelPressure:
          pressure = s.ReadUInt8();
          return new ChannelPressureEvent(deltaTime, channel, pressure);
        case EventType.PitchBend:
          pitchBend = s.ReadUInt16LE();
          return new PitchBendEvent(deltaTime, channel, pitchBend);
      }
      if(status == 0xFF) // meta event
      {
        byte type = s.ReadUInt8();
        int length = s.ReadMidiMultiByte();
        byte[] tmp;
        switch (type)
        {
          case 0x00:
            if (length != 2)
              throw new InvalidDataException("Sequence number events must have 2 bytes of data; this one has " + length);
            return new SequenceNumber(deltaTime, s.ReadUInt16BE());
          case 0x01:
            return new TextEvent(deltaTime, Encoding.ASCII.GetString(s.ReadBytes(length)));
          case 0x02:
            return new CopyrightNotice(deltaTime, Encoding.ASCII.GetString(s.ReadBytes(length)));
          case 0x03:
            return new TrackName(deltaTime, Encoding.ASCII.GetString(s.ReadBytes(length)));
          case 0x04:
            return new InstrumentName(deltaTime, Encoding.ASCII.GetString(s.ReadBytes(length)));
          case 0x05:
            return new Lyric(deltaTime, Encoding.ASCII.GetString(s.ReadBytes(length)));
          case 0x06:
            return new Marker(deltaTime, Encoding.ASCII.GetString(s.ReadBytes(length)));
          case 0x07:
            return new CuePoint(deltaTime, Encoding.ASCII.GetString(s.ReadBytes(length)));
          case 0x20:
            if (length != 1)
              throw new InvalidDataException("Channel prefix events must have 1 byte of data; this one has " + length);
            return new ChannelPrefix(deltaTime, s.ReadUInt8());
          case 0x2F:
            return new EndOfTrackEvent(deltaTime);
          case 0x51:
            if (length != 3)
              throw new InvalidDataException("Tempo events must have 3 bytes of data; this one has " + length);
            return new TempoEvent(deltaTime, s.ReadUInt24BE());
          case 0x54:
            if (length != 5)
              throw new InvalidDataException("SMTPE Offset events must have 5 bytes of data; this one has " + length);
            tmp = s.ReadBytes(length);
            return new SmtpeOffset(deltaTime, tmp[0], tmp[1], tmp[2], tmp[3], tmp[4]);
          case 0x58:
            if (length != 4)
              throw new InvalidDataException("Time Signature events must have 4 bytes of data; this one has " + length);
            tmp = s.ReadBytes(length);
            return new TimeSignature(deltaTime, tmp[0], tmp[1], tmp[2], tmp[3]);
          case 0x59:
            if (length != 2)
              throw new InvalidDataException("Key Signature events must have 2 bytes of data; this one has " + length);
            tmp = s.ReadBytes(length);
            return new KeySignature(deltaTime, tmp[0], tmp[1]);
          case 0x7F:
            return new SequencerSpecificEvent(deltaTime, s.ReadBytes(length));
          default: // unknown meta event, just skip past it.
            s.Position += length;
            return new MetaEvent(deltaTime);
        }
      }
      else // sysex
      {
        if (status == 0xF0) // should prefix Sysex with F0 (start-of-exclusive)
        {
          byte[] data = new byte[s.ReadMidiMultiByte() + 1];
          data[0] = 0xF0;
          s.Read(data, 1, data.Length - 1);
          return new SysexEvent(deltaTime, data);
        }
        else
        {
          return new SysexEvent(deltaTime, s.ReadBytes(s.ReadMidiMultiByte()));
        }
      }
    }

    /// <summary>
    /// Number of ticks between this and the last event.
    /// </summary>
    public int DeltaTime { get; }

    internal MidiMessage(int deltaTime)
    {
      DeltaTime = deltaTime;
    }
  }

  enum EventType : byte
  {
    NoteOff = 0x80,
    NoteOn = 0x90,
    NotePresure = 0xA0,
    Controller = 0xB0,
    ProgramChange = 0xC0,
    ChannelPressure = 0xD0,
    PitchBend = 0xE0,
    MetaOrSysex = 0xF0
  }
}
