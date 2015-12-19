/*
 * MidiTrack.cs
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
  class MidiTrack
  {
    public static MidiTrack ReadFrom(Stream stream)
    {
      if (stream.ReadInt32BE() != 0x4D54726B)
        throw new InvalidDataException("MIDI track not recognized.");
      long trkLen = stream.ReadUInt32BE();
      List<MidiMessage> messages = new List<MidiMessage>();
      long totalTicks = 0;
      while (trkLen > 0)
      {
        long pos = stream.Position;
        var newMsg = MidiMessage.ReadFrom(stream);
        messages.Add(newMsg);
        totalTicks += newMsg.DeltaTime;
        trkLen -= stream.Position - pos; // subtract message length from total track length
      }
      return new MidiTrack(messages, totalTicks);
    }

    public long TotalTicks { get; }

    private List<MidiMessage> _messages;
    public List<MidiMessage> Messages => _messages;
    public MidiTrack(List<MidiMessage> messages, long totalTicks)
    {
      _messages = messages;
      TotalTicks = totalTicks;
    }
  }
}
