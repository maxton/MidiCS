﻿/*
 * midifile.cs
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
using System.Linq;

namespace MidiCS
{

  public class MidiFile
  {
    public MidiFormat Format => _format;
    public double Duration { get; }
    public IList<TimeSigTempoEvent> TempoTimeSigMap => _tempoTimeSigMap;
    public ushort TicksPerQN => _ticksPerQn;
    public List<MidiTrack> Tracks => _tracks;

    /// <summary>
    /// Tries to find the track whose name matches. Otherwise null is returned.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public MidiTrack GetTrackByName(string name)
    {
      foreach(var track in _tracks)
      {
        if (name == track.Name)
          return track;
      }
      return null;
    }

    private MidiFormat _format;
    private List<MidiTrack> _tracks;
    private List<TimeSigTempoEvent> _tempoTimeSigMap;
    private ushort _ticksPerQn;

    public MidiFile(MidiFormat format, List<MidiTrack> tracks, ushort ticksPerqn)
    {
      _tracks = tracks ?? throw new ArgumentException("Tracks must not be null", nameof(tracks));
      if(_tracks.Count == 0)
      {
        throw new ArgumentException("No tracks provided to Midi File", nameof(tracks));
      }
      _format = format;
      _ticksPerQn = ticksPerqn;
      Duration = ProcessTempoMap();
    }

    /// <summary>
    /// Process the tempo map, also calculate the duration of the file.
    /// </summary>
    /// <returns></returns>
    private double ProcessTempoMap()
    {
      double duration = 0;
      long ticks = 0; // running total of MIDI ticks
      int tempo = 500000; // 500,000 microseconds per beat

      // tempo+timesig map stuff
      _tempoTimeSigMap = new List<TimeSigTempoEvent>();
      var tempos = new Dictionary<long, Events.TempoEvent>();
      var sigs = new Dictionary<long, Events.TimeSignature>();
      var durations = new SortedDictionary<long, double>();

      foreach (IMidiMessage m in _tracks[0].Messages) // tempo map track
      {
        ticks += m.DeltaTime;
        duration += (m.DeltaTime / (double)_ticksPerQn) * (tempo / 1000000.0);
        if (m is Events.TempoEvent)
        {
          tempo = (int)(m as Events.TempoEvent).MicrosPerQn;
          if (tempos.ContainsKey(ticks))
            tempos[ticks] = m as Events.TempoEvent;
          else
            tempos.Add(ticks, m as Events.TempoEvent);
          if (!durations.ContainsKey(ticks))
            durations.Add(ticks, duration);
        }
        else if (m is Events.TimeSignature)
        {
          if (sigs.ContainsKey(ticks))
            sigs[ticks] = m as Events.TimeSignature;
          else
            sigs.Add(ticks, m as Events.TimeSignature);
          if (!durations.ContainsKey(ticks))
            durations.Add(ticks, duration);
        }
      }
      // calculate length for tracks extending past tempo map
      for(var i = 1; i < _tracks.Count; i++)
      {
        if (_tracks[i].TotalTicks <= ticks) continue;
        long tmpTicks = 0;
        foreach (IMidiMessage m in _tracks[i].Messages)
        {
          tmpTicks += m.DeltaTime;
          if (tmpTicks < ticks) continue;
          ticks += m.DeltaTime;
          duration += (m.DeltaTime / (double)_ticksPerQn) * (tempo / 1000000.0);
        }
      }

      // more tempo+timesig map stuff
      double lastTempo = 120.0;
      Events.TempoEvent te;
      Events.TimeSignature ts;
      double time = 0.0;
      foreach (long tick in durations.Keys)
      {
        durations.TryGetValue(tick, out time);
        if (tempos.ContainsKey(tick) && sigs.ContainsKey(tick))
        {
          tempos.TryGetValue(tick, out te);
          sigs.TryGetValue(tick, out ts);
          lastTempo = 60.0 / (te.MicrosPerQn / 1000000.0);
          _tempoTimeSigMap.Add(new TimeSigTempoEvent(time, lastTempo, true, true, ts.Numerator, (byte)(1 << ts.Denominator), tick));
        }
        else if (tempos.ContainsKey(tick))
        {
          tempos.TryGetValue(tick, out te);
          lastTempo = 60.0 / (te.MicrosPerQn / 1000000.0);
          var lastTS = _tempoTimeSigMap.Last();
          _tempoTimeSigMap.Add(new TimeSigTempoEvent(time, lastTempo, false, true, lastTS.Numerator, lastTS.Denominator, tick));
        }
        else if (sigs.ContainsKey(tick))
        {
          sigs.TryGetValue(tick, out ts);
          _tempoTimeSigMap.Add(new TimeSigTempoEvent(time, lastTempo, true, false, ts.Numerator, (byte)(1 << ts.Denominator), tick));
        }
      }
      return duration;
    }
  }

  public class TimeSigTempoEvent
  {
    /// <summary>
    /// The time, in seconds, where this tempo change occurs.
    /// </summary>
    public double Time { get; }
    /// <summary>
    /// The MIDI tick at which this tempo change occurs.
    /// </summary>
    public long Tick { get; }
    /// <summary>
    /// The tempo that follows this marker.
    /// </summary>
    public double BPM { get; }
    /// <summary>
    /// True if this marker defines a new time signature.
    /// </summary>
    public bool NewTimeSig { get; }
    /// <summary>
    /// True if this marker defines a new tempo.
    /// </summary>
    public bool NewTempo { get; }
    /// <summary>
    /// The numerator of the time signature, if this marker defines a new time signature.
    /// </summary>
    public byte Numerator { get; }
    /// <summary>
    /// The denominator of the time signature, if this marker defines a new time signature.
    /// </summary>
    public byte Denominator { get; }
    public TimeSigTempoEvent(double time, double bpm, bool newtimesig, bool newTempo, byte num, byte denom, long ticks)
    {
      Time = time;
      Tick = ticks;
      BPM = bpm;
      NewTimeSig = newtimesig;
      NewTempo = newTempo;
      Numerator = num;
      Denominator = denom;
    }
  }

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
}
