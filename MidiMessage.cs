/*
 * MidiMessage.cs
 * 
 * Copyright (c) 2015,2016, maxton. All rights reserved.
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
  public interface IMidiMessage
  {
    /// <summary>
    /// Number of ticks between this and the last event.
    /// </summary>
    uint DeltaTime { get; }

    /// <summary>
    /// The type of this event.
    /// </summary>
    EventType Type { get; }

    /// <summary>
    /// A human-readable representation of the message.
    /// </summary>
    string PrettyString { get; }
  }

  public enum EventType : byte
  {
    NoteOff = 0x80,
    NoteOn = 0x90,
    NotePresure = 0xA0,
    Controller = 0xB0,
    ProgramChange = 0xC0,
    ChannelPressure = 0xD0,
    PitchBend = 0xE0,
    MetaEvent = 0xFF,
    Sysex = 0xF0,
    SysexRaw = 0xF7
  }
  public enum MetaEventType : byte
  {
    SequenceNumber = 0x00,
    TextEvent = 0x01,
    CopyrightNotice = 0x02,
    TrackName = 0x03,
    InstrumentName = 0x04,
    Lyric = 0x05,
    Marker = 0x06,
    CuePoint = 0x07,
    ChannelPrefix = 0x20,
    EndOfTrack = 0x2F,
    TempoEvent = 0x51,
    SmtpeOffset = 0x54,
    TimeSignature = 0x58,
    KeySignature = 0x59,
    SequencerSpecific = 0x7F
  }
}
