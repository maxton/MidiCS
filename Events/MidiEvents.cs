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
  public interface IMidiEvent : IMidiMessage
  {
    byte Channel { get; }
  }

  public class NoteOnEvent : IMidiEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.NoteOn;
    public byte Channel { get; }
    public byte Key { get; }
    public byte Velocity { get; }
    internal NoteOnEvent(int deltaTime, byte channel, byte key, byte velocity)
    {
      DeltaTime = deltaTime;
      Channel = channel;
      Key = key;
      Velocity = velocity;
    }
  }
  public class NoteOffEvent : IMidiEvent
  {
    public int DeltaTime { get; }
    public byte Channel { get; }
    public EventType Type => EventType.NoteOff;
    public byte Key { get; }
    public byte Velocity { get; }
    internal NoteOffEvent(int deltaTime, byte channel, byte key, byte velocity)
    {
      DeltaTime = deltaTime;
      Channel = channel;
      Key = key;
      Velocity = velocity;
    }
  }
  public class NotePressureEvent : IMidiEvent
  {
    public int DeltaTime { get; }
    public byte Channel { get; }
    public EventType Type => EventType.NotePresure;
    public byte Key { get; }
    public byte Pressure { get; }
    internal NotePressureEvent(int deltaTime, byte channel, byte key, byte pressure)
    {
      DeltaTime = deltaTime;
      Channel = channel;
      Key = key;
      Pressure = pressure;
    }
  }
  public class ControllerEvent : IMidiEvent
  {
    public int DeltaTime { get; }
    public byte Channel { get; }
    public EventType Type => EventType.Controller;
    public byte Controller { get; }
    public byte Value { get; }
    internal ControllerEvent(int deltaTime, byte channel, byte controller, byte value)
    {
      DeltaTime = deltaTime;
      Channel = channel;
      Controller = controller;
      Value = value;
    }
  }
  public class ProgramChgEvent : IMidiEvent
  {
    public int DeltaTime { get; }
    public byte Channel { get; }
    public EventType Type => EventType.ProgramChange;
    public byte Program { get; }
    internal ProgramChgEvent(int deltaTime, byte channel, byte program)
    {
      DeltaTime = deltaTime;
      Channel = channel;
      Program = program;
    }
  }
  public class ChannelPressureEvent : IMidiEvent
  {
    public int DeltaTime { get; }
    public byte Channel { get; }
    public EventType Type => EventType.ChannelPressure;
    public byte Pressure { get; }
    internal ChannelPressureEvent(int deltaTime, byte channel, byte pressure)
    {
      DeltaTime = deltaTime;
      Channel = channel;
      Pressure = pressure;
    }
  }
  public class PitchBendEvent : IMidiEvent
  {
    public int DeltaTime { get; }
    public byte Channel { get; }
    public EventType Type => EventType.PitchBend;
    public ushort Bend { get; }
    internal PitchBendEvent(int deltaTime, byte channel, ushort bend)
    {
      DeltaTime = deltaTime;
      Channel = channel;
      Bend = bend;
    }
  }
}
