/*
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
  public abstract class MidiEvent : MidiMessage
  {
    public byte Channel { get; }
    internal MidiEvent(int deltaTime, byte channel) : base(deltaTime)
    {
      Channel = channel;
    }
  }

  public class NoteOnEvent : MidiEvent
  {
    public override EventType Type => EventType.NoteOn;
    public byte Key { get; }
    public byte Velocity { get; }
    internal NoteOnEvent(int deltaTime, byte channel, byte key, byte velocity) : base(deltaTime, channel)
    { Key = key; Velocity = velocity; }
  }
  public class NoteOffEvent : MidiEvent
  {
    public override EventType Type => EventType.NoteOff;
    public byte Key { get; }
    public byte Velocity { get; }
    internal NoteOffEvent(int deltaTime, byte channel, byte key, byte velocity) : base(deltaTime, channel)
    { Key = key; Velocity = velocity; }
  }
  public class NotePressureEvent : MidiEvent
  {
    public override EventType Type => EventType.NotePresure;
    public byte Key { get; }
    public byte Pressure { get; }
    internal NotePressureEvent(int deltaTime, byte channel, byte key, byte pressure) : base(deltaTime, channel)
    { Key = key; Pressure = pressure; }
  }
  public class ControllerEvent : MidiEvent
  {
    public override EventType Type => EventType.Controller;
    public byte Controller { get; }
    public byte Value { get; }
    internal ControllerEvent(int deltaTime, byte channel, byte controller, byte value) : base(deltaTime, channel)
    { Controller = controller; Value = value; }
  }
  public class ProgramChgEvent : MidiEvent
  {
    public override EventType Type => EventType.ProgramChange;
    public byte Program { get; }
    internal ProgramChgEvent(int deltaTime, byte channel, byte program) : base(deltaTime, channel)
    { Program = program; }
  }
  public class ChannelPressureEvent : MidiEvent
  {
    public override EventType Type => EventType.ChannelPressure;
    public byte Pressure { get; }
    internal ChannelPressureEvent(int deltaTime, byte channel, byte pressure) : base(deltaTime, channel)
    { Pressure = pressure; }
  }
  public class PitchBendEvent : MidiEvent
  {
    public override EventType Type => EventType.PitchBend;
    public ushort Bend { get; }
    internal PitchBendEvent(int deltaTime, byte channel, ushort bend) : base(deltaTime, channel)
    { Bend = bend; }
  }
}
