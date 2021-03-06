﻿/*
 * SysexEvents.cs
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

namespace MidiCS.Events
{
  public class SysexEvent : IMidiMessage
  {
    public uint DeltaTime { get; }
    public EventType Type => EventType.SysexRaw;
    public byte[] Data { get; }
    public string PrettyString => $"Sysex: {Data.Length} bytes";
    public SysexEvent(uint deltaTime, byte[] sysexData)
    {
      DeltaTime = deltaTime;
      Data = sysexData;
    }
  }
}