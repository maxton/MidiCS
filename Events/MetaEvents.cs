﻿/*
 * metaevents.cs
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
  public interface MetaEvent : IMidiMessage
  {
    MetaEventType MetaType { get; }
  }
  public interface MetaTextEvent : MetaEvent
  {
    string Text { get; }
  }
  public class SequenceNumber : MetaEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public MetaEventType MetaType => MetaEventType.SequenceNumber;
    public ushort Number { get; }
    public string PrettyString => $"SequenceNumber: {Number}";
    internal SequenceNumber(int deltaTime, ushort number)
    {
      DeltaTime = deltaTime;
      Number = number;
    }
  }
  public class TextEvent : MetaTextEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public string Text { get; }
    public MetaEventType MetaType => MetaEventType.TextEvent;
    public string PrettyString => $"TextEvent: {Text}";
    internal TextEvent(int deltaTime, string text)
    {
      DeltaTime = deltaTime;
      Text = text;
    }
  }
  public class CopyrightNotice : MetaTextEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public string Text { get; }
    public MetaEventType MetaType => MetaEventType.CopyrightNotice;
    public string PrettyString => $"CopyrightNotice: {Text}";
    internal CopyrightNotice(int deltaTime, string text)
    {
      DeltaTime = deltaTime;
      Text = text;
    }
  }
  public class TrackName : MetaTextEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public string Text { get; }
    public MetaEventType MetaType => MetaEventType.TrackName;
    public string PrettyString => $"TrackName: {Text}";
    internal TrackName(int deltaTime, string name)
    {
      DeltaTime = deltaTime;
      Text = name;
    }
  }
  public class InstrumentName : MetaTextEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public string Text { get; }
    public MetaEventType MetaType => MetaEventType.InstrumentName;
    public string PrettyString => $"InstrumentName: {Text}";
    internal InstrumentName(int deltaTime, string name)
    {
      DeltaTime = deltaTime;
      Text = name;
    }
  }
  public class Lyric : MetaTextEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public string Text { get; }
    public MetaEventType MetaType => MetaEventType.Lyric;
    public string PrettyString => $"Lyric: {Text}";
    internal Lyric(int deltaTime, string text)
    {
      DeltaTime = deltaTime;
      Text = text;
    }
  }
  public class Marker : MetaTextEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public string Text { get; }
    public MetaEventType MetaType => MetaEventType.Marker;
    public string PrettyString => $"Marker: {Text}";
    internal Marker(int deltaTime, string text)
    {
      DeltaTime = deltaTime;
      Text = text;
    }
  }
  public class CuePoint : MetaTextEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public string Text { get; }
    public MetaEventType MetaType => MetaEventType.CuePoint;
    public string PrettyString => $"CuePoint: {Text}";
    internal CuePoint(int deltaTime, string text)
    {
      DeltaTime = deltaTime;
      Text = text;
    }
  }
  public class ChannelPrefix : MetaEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public MetaEventType MetaType => MetaEventType.ChannelPrefix;
    public byte Channel { get; }
    public string PrettyString => $"ChannelPrefix: {Channel}";
    internal ChannelPrefix(int deltaTime, byte channel)
    {
      DeltaTime = deltaTime;
      Channel = channel;
    }
  }
  public class EndOfTrackEvent : MetaEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public MetaEventType MetaType => MetaEventType.EndOfTrack;
    public string PrettyString => $"EndOfTrack";
    internal EndOfTrackEvent(int deltaTime)
    {
      DeltaTime = deltaTime;
    }
  }
  public class TempoEvent : MetaEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public MetaEventType MetaType => MetaEventType.TempoEvent;
    public int MicrosPerQn { get; }
    public string PrettyString => $"TempoEvent: {MicrosPerQn}";
    internal TempoEvent(int deltaTime, int microsPerQn)
    {
      DeltaTime = deltaTime;
      MicrosPerQn = microsPerQn;
    }
  }
  public class SmtpeOffset : MetaEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public MetaEventType MetaType => MetaEventType.SmtpeOffset;
    public byte Hours { get; }
    public byte Minutes { get; }
    public byte Seconds { get; }
    public byte Frames { get; }
    public byte FrameHundredths { get; }
    public string PrettyString => $"SmtpeOffset: {Hours}:{Minutes}:{Seconds}::{Frames}.{FrameHundredths}";
    internal SmtpeOffset(int deltaTime, byte h, byte m, byte s, byte f, byte ff)
    {
      DeltaTime = deltaTime;
      Hours = h;
      Minutes = m;
      Seconds = s;
      Frames = f;
      FrameHundredths = ff;
    }
  }
  public class TimeSignature : MetaEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public MetaEventType MetaType => MetaEventType.TimeSignature;
    public byte Numerator { get; }
    public byte Denominator { get; }
    public byte ClocksPerTick { get; }
    public byte ThirtySecondNotesPer24Clocks { get; }
    public string PrettyString => $"TimeSignature: {Numerator}/{Denominator} @ {ClocksPerTick}, {ThirtySecondNotesPer24Clocks}";
    internal TimeSignature(int deltaTime, byte num, byte denom, byte clocksPerTick, byte thirtySecondNotesPer24Clocks)
    {
      DeltaTime = deltaTime;
      Numerator = num;
      Denominator = denom;
      ClocksPerTick = clocksPerTick;
      ThirtySecondNotesPer24Clocks = thirtySecondNotesPer24Clocks;
    }
  }
  public class KeySignature : MetaEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public MetaEventType MetaType => MetaEventType.KeySignature;
    public byte Sharps { get; }
    public byte Tonality { get; }
    public string PrettyString => $"KeySignature: {Sharps} sharps, {Tonality} tonality";
    internal KeySignature(int deltaTime, byte sharps, byte tonality)
    {
      DeltaTime = deltaTime;
      Sharps = sharps;
      Tonality = tonality;
    }
  }
  public class SequencerSpecificEvent : MetaEvent
  {
    public int DeltaTime { get; }
    public EventType Type => EventType.MetaEvent;
    public MetaEventType MetaType => MetaEventType.SequencerSpecific;
    public byte[] Data { get; }
    public string PrettyString => $"SequencerSpecificEvent: {Data.Length} bytes";
    internal SequencerSpecificEvent(int deltaTime, byte[] data)
    {
      DeltaTime = deltaTime;
      Data = data;
    }
  }
}
