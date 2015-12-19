/*
 * MidiFile.cs
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
using System.Collections.Generic;
using System.IO;

namespace MidiCS
{
  /// <summary>
  /// The format of the MIDI file.
  /// </summary>
  public enum MidiFormat : ushort
  {
    /// <summary>
    /// Midi file contains one track.
    /// </summary>
    Single = 0x0,

    /// <summary>
    /// Midi file contains multiple tracks.
    /// </summary>
    MultiTrack = 0x1

      // note: format 2 is NOT supported. (for now)
  }

  public class MidiFile
  {
    public static MidiFile FromBytes(byte[] bytes)
    {
      using(var s = new MemoryStream(bytes))
        return new MidiFile(s);
    }
    public static MidiFile FromStream(Stream stream)
    {
      return new MidiFile(stream);
    }

    private MidiFormat _format;
    public MidiFormat Format => _format;

    private List<MidiTrack> _tracks;

    public double Duration { get; }

    private ushort _ticksPerQn;

    private MidiFile(Stream stream)
    {
      readHeader(stream);
      readTracks(stream);
      Duration = CalcDuration();
    }

    private void readHeader(Stream stream)
    {
      // "MThd" big-endian, header size always = 6
      if (stream.ReadInt32BE() != 0x4D546864 || stream.ReadInt32BE() != 0x6) 
        throw new InvalidDataException("MIDI file did not begin with proper MIDI header.");
      _format = (MidiFormat)stream.ReadUInt16BE();
      if (_format > MidiFormat.MultiTrack)
        throw new NotSupportedException("MIDI format " + _format + " is not supported by this library.");
      _tracks = new List<MidiTrack>(stream.ReadUInt16BE());
      _ticksPerQn = stream.ReadUInt16BE();
      if ((_ticksPerQn & 0x8000) == 0x8000)
        throw new NotSupportedException("SMPTE delta time format is not supported by this library.");
    }

    private void readTracks(Stream stream)
    {
      for(int i = 0; i < _tracks.Capacity; i++)
      {
        _tracks.Add(MidiTrack.ReadFrom(stream));
      }
    }

    private double CalcDuration()
    {
      double duration = 0;
      long ticks = 0; // running total of MIDI ticks
      int tempo = 500000; // 500,000 microseconds per beat
      foreach(MidiMessage m in _tracks[0].Messages) // tempo map track
      {
        ticks += m.DeltaTime;
        duration += (m.DeltaTime / (double)_ticksPerQn) * (tempo / 1000000.0);
        if(m is Events.TempoEvent)
        {
          tempo = (m as Events.TempoEvent).MicrosPerQn;
        }
      }
      for(var i = 1; i < _tracks.Count; i++)
      {
        if (_tracks[i].TotalTicks <= ticks) continue;
        long tmpTicks = 0;
        foreach (MidiMessage m in _tracks[i].Messages)
        {
          tmpTicks += m.DeltaTime;
          if (tmpTicks < ticks) continue;
          ticks += m.DeltaTime;
          duration += (m.DeltaTime / (double)_ticksPerQn) * (tempo / 1000000.0);
        }
      }
      return duration;
    }
  }
}
