﻿/*
 * MidiEvents.cs
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

namespace MidiCS.Events
{
  abstract class MidiEvent : MidiMessage
  {
    public byte Channel { get; }
    internal MidiEvent(int deltaTime, byte channel) : base(deltaTime)
    {
      Channel = channel;
    }
  }

  class NoteOnEvent : MidiEvent
  {
    public byte Key { get; }
    public byte Velocity { get; }
    internal NoteOnEvent(int deltaTime, byte channel, byte key, byte velocity) : base(deltaTime, channel)
    { Key = key; Velocity = velocity; }
  }
  class NoteOffEvent : MidiEvent
  {
    public byte Key { get; }
    public byte Velocity { get; }
    internal NoteOffEvent(int deltaTime, byte channel, byte key, byte velocity) : base(deltaTime, channel)
    { Key = key; Velocity = velocity; }
  }
  class NotePressureEvent : MidiEvent
  {
    public byte Key { get; }
    public byte Pressure { get; }
    internal NotePressureEvent(int deltaTime, byte channel, byte key, byte pressure) : base(deltaTime, channel)
    { Key = key; Pressure = pressure; }
  }
  class ControllerEvent : MidiEvent
  {
    public byte Controller { get; }
    public byte Value { get; }
    internal ControllerEvent(int deltaTime, byte channel, byte controller, byte value) : base(deltaTime, channel)
    { Controller = controller; Value = value; }
  }
  class ProgramChgEvent : MidiEvent
  {
    public byte Program { get; }
    internal ProgramChgEvent(int deltaTime, byte channel, byte program) : base(deltaTime, channel)
    { Program = program; }
  }
  class ChannelPressureEvent : MidiEvent
  {
    public byte Pressure { get; }
    internal ChannelPressureEvent(int deltaTime, byte channel, byte pressure) : base(deltaTime, channel)
    { Pressure = pressure; }
  }
  class PitchBendEvent : MidiEvent
  {
    public ushort Bend { get; }
    internal PitchBendEvent(int deltaTime, byte channel, ushort bend) : base(deltaTime, channel)
    { Bend = bend; }
  }
}