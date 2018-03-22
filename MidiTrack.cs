/*
 * MidiTrack.cs
 * 
 * Copyright (c) 2015,2016,2018 maxton. All rights reserved.
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
  public class MidiTrack
  {
    public long TotalTicks { get; }
    public string Name { get; }
    public List<IMidiMessage> Messages { get; }

    internal MidiTrack(List<IMidiMessage> messages, long totalTicks, string name)
    {
      Name = name;
      Messages = messages;
      TotalTicks = totalTicks;
    }
  }
}
