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
  class MetaEvent : MidiMessage
  {
    internal MetaEvent(int deltaTime) : base(deltaTime)
    { }
  }
  abstract class MetaTextEvent : MetaEvent
  {
    string Text { get; }
    internal MetaTextEvent(int deltaTime, string text) : base(deltaTime)
    {
      Text = text;
    }
  }
  class SequenceNumber : MetaEvent
  {
    internal SequenceNumber(int deltaTime, ushort number) : base(deltaTime)
    { }
  }
  class TextEvent : MetaTextEvent
  {
    internal TextEvent(int deltaTime, string text) : base(deltaTime, text)
    { }
  }
  class CopyrightNotice : MetaTextEvent
  {
    internal CopyrightNotice(int deltaTime, string text) : base(deltaTime, text)
    { }
  }
  class TrackName : MetaTextEvent
  {
    internal TrackName(int deltaTime, string name) : base(deltaTime, name)
    {  }
  }
  class InstrumentName : MetaTextEvent
  {
    internal InstrumentName(int deltaTime, string name) : base(deltaTime, name)
    { }
  }
  class Lyric : MetaTextEvent
  {
    internal Lyric(int deltaTime, string text) : base(deltaTime, text)
    { }
  }
  class Marker : MetaTextEvent
  {
    internal Marker(int deltaTime, string text) : base(deltaTime, text)
    { }
  }
  class CuePoint : MetaTextEvent
  {
    internal CuePoint(int deltaTime, string text) : base(deltaTime, text)
    { }
  }
  class ChannelPrefix : MetaEvent
  {
    internal ChannelPrefix(int deltaTime, byte channel) : base(deltaTime)
    { }
  }
  class EndOfTrackEvent : MetaEvent
  {
    internal EndOfTrackEvent(int deltaTime) : base(deltaTime)
    { }
  }
  class TempoEvent : MetaEvent
  {
    public int MicrosPerQn { get; }
    internal TempoEvent(int deltaTime, int microsPerQn) : base(deltaTime)
    { MicrosPerQn = microsPerQn; }
  }
  class SmtpeOffset : MetaEvent
  {
    internal SmtpeOffset(int deltaTime, byte h, byte m, byte s, byte f, byte ff) : base(deltaTime)
    { }
  }
  class TimeSignature : MetaEvent
  {
    internal TimeSignature(int deltaTime, byte num, byte denom, byte clocksPerTick, byte thirtySecondNotesPer24Clocks) : base(deltaTime)
    { }
  }
  class KeySignature : MetaEvent
  {
    internal KeySignature(int deltaTime, byte sharps, byte tonality) : base(deltaTime)
    { }
  }
  class SequencerSpecificEvent : MetaEvent
  {
    internal SequencerSpecificEvent(int deltaTime, byte[] data) : base(deltaTime)
    { }
  }
}
