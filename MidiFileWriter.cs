using System.IO;
using MidiCS.Events;

namespace MidiCS
{
  public class MidiFileWriter
  {
    public static void WriteSMF(MidiFile f, Stream s)
    {
      s.WriteBE(0x4D546864);
      s.WriteBE(0x6);
      s.WriteBE((ushort)f.Format);
      s.WriteBE((ushort)f.Tracks.Count);
      s.WriteBE(f.TicksPerQN);
      foreach(var t in f.Tracks)
      {
        s.WriteBE(0x4D54726B);
        using (var ms = new MemoryStream(t.Messages.Count * 3))
        {
          foreach(var msg in t.Messages)
          {
            writeMessage(msg, ms);
          }
          // Write track length.
          s.WriteBE((int)ms.Length);
          ms.Position = 0;
          // Write messages.
          ms.WriteTo(s);
        }
      }
    }

    static int tick = 0;
    static byte running_status = 0;
    static void writeMessage(IMidiMessage m, Stream s)
    {
      s.WriteMidiMultiByte(m.DeltaTime);
      switch (m)
      {
        case IMidiEvent e:
          var status = (byte)(e.Channel | (byte)e.Type);
          if(status != running_status)
          {
            s.WriteByte(status);
            running_status = status;
          }
          switch(e)
          {
            case NoteOffEvent x:
              s.WriteByte(x.Key);
              s.WriteByte(x.Velocity);
              break;
            case NoteOnEvent x:
              s.WriteByte(x.Key);
              s.WriteByte(x.Velocity);
              break;
            case NotePressureEvent x:
              s.WriteByte(x.Key);
              s.WriteByte(x.Pressure);
              break;
            case ControllerEvent x:
              s.WriteByte(x.Controller);
              s.WriteByte(x.Value);
              break;
            case ProgramChgEvent x:
              s.WriteByte(x.Program);
              break;
            case ChannelPressureEvent x:
              s.WriteByte(x.Pressure);
              break;
            case PitchBendEvent x:
              s.WriteBE(x.Bend);
              break;
          }
          break;
        case MetaEvent e:
          // cancel running status
          running_status = 0;
          s.WriteByte(0xFF);
          s.WriteByte((byte)e.MetaType);
          switch(e)
          {
            case SequenceNumber x:
              s.WriteMidiMultiByte(2);
              s.WriteBE(x.Number);
              break;
            case TextEvent x:
              s.WriteMidiMultiByte((uint)x.Text.Length);
              s.Write(System.Text.Encoding.ASCII.GetBytes(x.Text), 0, x.Text.Length);
              break;
            case CopyrightNotice x:
              s.WriteMidiMultiByte((uint)x.Text.Length);
              s.Write(System.Text.Encoding.ASCII.GetBytes(x.Text), 0, x.Text.Length);
              break;
            case TrackName x:
              s.WriteMidiMultiByte((uint)x.Text.Length);
              s.Write(System.Text.Encoding.ASCII.GetBytes(x.Text), 0, x.Text.Length);
              break;
            case InstrumentName x:
              s.WriteMidiMultiByte((uint)x.Text.Length);
              s.Write(System.Text.Encoding.ASCII.GetBytes(x.Text), 0, x.Text.Length);
              break;
            case Lyric x:
              s.WriteMidiMultiByte((uint)x.Text.Length);
              s.Write(System.Text.Encoding.ASCII.GetBytes(x.Text), 0, x.Text.Length);
              break;
            case Marker x:
              s.WriteMidiMultiByte((uint)x.Text.Length);
              s.Write(System.Text.Encoding.ASCII.GetBytes(x.Text), 0, x.Text.Length);
              break;
            case CuePoint x:
              s.WriteMidiMultiByte((uint)x.Text.Length);
              s.Write(System.Text.Encoding.ASCII.GetBytes(x.Text), 0, x.Text.Length);
              break;
            case ChannelPrefix x:
              s.WriteMidiMultiByte(1);
              s.WriteByte(x.Channel);
              break;
            case EndOfTrackEvent x:
              s.WriteMidiMultiByte(0);
              break;
            case TempoEvent x:
              s.WriteMidiMultiByte(3);
              s.WriteUInt24BE(x.MicrosPerQn);
              break;
            case SmtpeOffset x:
              s.WriteMidiMultiByte(5);
              var sdata = new byte[5] { x.Hours, x.Minutes, x.Seconds, x.Frames, x.FrameHundredths };
              s.Write(sdata, 0, 5);
              break;
            case TimeSignature x:
              s.WriteMidiMultiByte(4);
              var tdata = new byte[4] { x.Numerator, x.Denominator, x.ClocksPerTick, x.ThirtySecondNotesPer24Clocks };
              s.Write(tdata, 0, 4);
              break;
            case KeySignature x:
              s.WriteMidiMultiByte(2);
              s.WriteByte(x.Sharps);
              s.WriteByte(x.Tonality);
              break;
            case SequencerSpecificEvent x:
              s.WriteMidiMultiByte((uint)x.Data.Length);
              s.Write(x.Data, 0, x.Data.Length);
              break;
          }
          break;
        case SysexEvent e:
          // cancel running status
          running_status = 0;
          s.WriteByte(0xF0);
          s.WriteMidiMultiByte((uint)e.Data.Length);
          s.Write(e.Data, 0, e.Data.Length);
          break;
      }
    }

  }
}
