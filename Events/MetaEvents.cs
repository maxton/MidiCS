/*
 * MetaEvents.cs
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
  public abstract class MetaEvent : MidiMessage
  {
    public override EventType Type => EventType.MetaEvent;
    public abstract MetaEventType MetaType { get; }
    internal MetaEvent(int deltaTime) : base(deltaTime)
    { }
  }
  public abstract class MetaTextEvent : MetaEvent
  {
    public string Text { get; }
    internal MetaTextEvent(int deltaTime, string text) : base(deltaTime)
    {
      Text = text;
    }
  }
  public class SequenceNumber : MetaEvent
  {
    public override MetaEventType MetaType => MetaEventType.SequenceNumber;
    public ushort Number { get; }
    internal SequenceNumber(int deltaTime, ushort number) : base(deltaTime)
    { Number = number; }
  }
  public class TextEvent : MetaTextEvent
  {
    public override MetaEventType MetaType => MetaEventType.TextEvent;
    internal TextEvent(int deltaTime, string text) : base(deltaTime, text)
    { }
  }
  public class CopyrightNotice : MetaTextEvent
  {
    public override MetaEventType MetaType => MetaEventType.CopyrightNotice;
    internal CopyrightNotice(int deltaTime, string text) : base(deltaTime, text)
    { }
  }
  public class TrackName : MetaTextEvent
  {
    public override MetaEventType MetaType => MetaEventType.TrackName;
    internal TrackName(int deltaTime, string name) : base(deltaTime, name)
    {  }
  }
  public class InstrumentName : MetaTextEvent
  {
    public override MetaEventType MetaType => MetaEventType.InstrumentName;
    internal InstrumentName(int deltaTime, string name) : base(deltaTime, name)
    { }
  }
  public class Lyric : MetaTextEvent
  {
    public override MetaEventType MetaType => MetaEventType.Lyric;
    internal Lyric(int deltaTime, string text) : base(deltaTime, text)
    { }
  }
  public class Marker : MetaTextEvent
  {
    public override MetaEventType MetaType => MetaEventType.Marker;
    internal Marker(int deltaTime, string text) : base(deltaTime, text)
    { }
  }
  public class CuePoint : MetaTextEvent
  {
    public override MetaEventType MetaType => MetaEventType.CuePoint;
    internal CuePoint(int deltaTime, string text) : base(deltaTime, text)
    { }
  }
  public class ChannelPrefix : MetaEvent
  {
    public override MetaEventType MetaType => MetaEventType.ChannelPrefix;
    public byte Channel { get; }
    internal ChannelPrefix(int deltaTime, byte channel) : base(deltaTime)
    { Channel = channel; }
  }
  public class EndOfTrackEvent : MetaEvent
  {
    public override MetaEventType MetaType => MetaEventType.EndOfTrack;
    internal EndOfTrackEvent(int deltaTime) : base(deltaTime)
    { }
  }
  public class TempoEvent : MetaEvent
  {
    public override MetaEventType MetaType => MetaEventType.TempoEvent;
    public int MicrosPerQn { get; }
    internal TempoEvent(int deltaTime, int microsPerQn) : base(deltaTime)
    { MicrosPerQn = microsPerQn; }
  }
  public class SmtpeOffset : MetaEvent
  {
    public override MetaEventType MetaType => MetaEventType.SmtpeOffset;
    public byte Hours { get; }
    public byte Minutes { get; }
    public byte Seconds { get; }
    public byte Frames { get; }
    public byte FrameHundredths { get; }
    internal SmtpeOffset(int deltaTime, byte h, byte m, byte s, byte f, byte ff) : base(deltaTime)
    {
      Hours = h;
      Minutes = m;
      Seconds = s;
      Frames = f;
      FrameHundredths = ff;
    }
  }
  public class TimeSignature : MetaEvent
  {
    public override MetaEventType MetaType => MetaEventType.TimeSignature;
    public byte Numerator { get; }
    public byte Denominator { get; }
    public byte ClocksPerTick { get; }
    public byte ThirtySecondNotesPer24Clocks { get; }
    internal TimeSignature(int deltaTime, byte num, byte denom, byte clocksPerTick, byte thirtySecondNotesPer24Clocks) : base(deltaTime)
    { Numerator = num; Denominator = denom;  ClocksPerTick = clocksPerTick; ThirtySecondNotesPer24Clocks = thirtySecondNotesPer24Clocks; }
  }
  public class KeySignature : MetaEvent
  {
    public override MetaEventType MetaType => MetaEventType.KeySignature;
    public byte Sharps { get; }
    public byte Tonality { get; }
    internal KeySignature(int deltaTime, byte sharps, byte tonality) : base(deltaTime)
    { Sharps = sharps; Tonality = tonality; }
  }
  public class SequencerSpecificEvent : MetaEvent
  {
    public override MetaEventType MetaType => MetaEventType.SequencerSpecific;
    public byte[] Data { get; }
    internal SequencerSpecificEvent(int deltaTime, byte[] data) : base(deltaTime)
    { Data = data; }
  }
}
