using Yukami.Extension;

namespace Yukami.Lib;

// `sub_40F860` in `it.exe`
public class SCRDumper
{
    private readonly BinaryReader _reader;
    public string[] Texts;
    public string[] FullCommands;
    
    public SCRDumper(MemoryStream reader)
    {
        _reader = new BinaryReader(reader);

        ParseContent();
    }
    
    
    // Explain command in the future if needed
    private void ParseContent()
    {
        var texts = new List<string>();
        var fullCommands = new List<string>();
        while (_reader.Now() < _reader.BaseStream.Length)
        {
            var opCode = _reader.ReadInt16Le();
            switch (opCode)
            {
                case 0x00:
                    if (_reader.Now() == _reader.BaseStream.Length)
                    {
                        break;
                    }

                    fullCommands.Add("Op 0x00");
                    break;
                case 0x01:
                    var op01Length = _reader.ReadInt16Le();
                    var op01Text = _reader.ReadBytesAsCString(op01Length);
                    fullCommands.Add($"Op 0x01, Length: {op01Length}, Text: {op01Text}");
                    break;
                case 0x02:
                    var op02Length = _reader.ReadInt16Le();
                    var op02Text = _reader.ReadBytesAsShiftJisString(op02Length);
                    // Console.WriteLine(op02Text);
                    fullCommands.Add($"Op 0x02, Length: {op02Length}, Text: {op02Text}");
                    texts.Add(op02Text);
                    break;
                case 0x03:
                    // What the f*ck is this?
                    fullCommands.Add("Op 0x03");
                    break;
                case 0x04:
                    var op04Value = _reader.ReadInt32Le();
                    fullCommands.Add($"Op 0x04, Value: {op04Value}");
                    break;
                case 0x05:
                    fullCommands.Add("Op 0x05");
                    break;
                case 0x06:
                    var op06Value = _reader.ReadInt32Le();
                    fullCommands.Add($"Op 0x06, Value: {op06Value}");
                    break;
                case 0x0B:
                    var op0BValue1 = _reader.ReadInt16Le();
                    var op0BValue2 = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x0B, Value: {op0BValue1}, {op0BValue2}");
                    break;
                case 0x0D:
                    fullCommands.Add("Op 0x0D");
                    break;
                case 0x0E:
                    var op0EValue = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x0E, Value: {op0EValue}");
                    break;
                case 0x0F:
                    var op0FValue = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x0F, Value: {op0FValue}");
                    break;
                case 0x10:
                    fullCommands.Add("Op 0x10");
                    break;
                case 0x11:
                    var op11Value = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x11, Value: {op11Value}");
                    break;
                case 0x12:
                    var op12Value = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x12, Value: {op12Value}");
                    break;
                case 0x13:
                    // Background?
                    var op13Length = _reader.ReadInt16Le();
                    var op13Text = _reader.ReadBytesAsCString(op13Length);
                    fullCommands.Add($"Op 0x13, Length: {op13Length}, Text: {op13Text}");
                    break;
                case 0x14:
                    var op14Value = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x14, Value: {op14Value}");
                    break;
                case 0x15:
                    var op15Value = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x15, Value: {op15Value}");
                    break;
                case 0x16:
                    var op16Length = _reader.ReadInt16Le();
                    var op16Text = _reader.ReadBytesAsShiftJisString(op16Length);
                    fullCommands.Add($"Op 0x16, Length: {op16Length}, Text: {op16Text}");
                    texts.Add(op16Text);
                    // Console.WriteLine(op16Text);
                    break;
                case 0x17:
                    var op17Value1 = _reader.ReadInt16Le();
                    var op17Value2 = _reader.ReadInt16Le();
                    var op17Value3 = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x17, Value: {op17Value1}, {op17Value2}, {op17Value3}");
                    break;
                case 0x18:
                    var op18Value = _reader.ReadInt16Le();
                    var op18Length = _reader.ReadInt16Le();
                    var op18Text = _reader.ReadBytesAsCString(op18Length);
                    fullCommands.Add($"Op 0x18, Value: {op18Value}, Length: {op18Length}, Text: {op18Text}");
                    break;
                case 0x19:
                    var op19Value = _reader.ReadInt16Le();
                    var op19Length = _reader.ReadInt16Le();
                    var op19Text = _reader.ReadBytesAsCString(op19Length);
                    fullCommands.Add($"Op 0x19, Value: {op19Value}, Length: {op19Length}, Text: {op19Text}");
                    break;
                case 0x1A:
                    var op1AValue = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x1A, Value: {op1AValue}");
                    break;
                case 0x1C:
                    var op1CValue = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x1C, Value: {op1CValue}");
                    break;
                case 0x1D:
                    fullCommands.Add("Op 0x1D");
                    break;
                case 0x1E:
                    var op1ELength = _reader.ReadInt16Le();
                    var op1EText = _reader.ReadBytesAsShiftJisString(op1ELength);
                    var op1ETextUnk = _reader.ReadBytesAsCString(4);
                    // Console.WriteLine(op1EText);
                    fullCommands.Add($"Op 0x1E, Length: {op1ELength}, Text: {op1EText}, Unk: {op1ETextUnk}");
                    texts.Add(op1EText);
                    break;
                case 0x1F:
                    // What the f*ck is this?
                    fullCommands.Add("Op 0x1F");
                    break;
                case 0x20:
                    var op20Value = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x20, Value: {op20Value}");
                    break;
                case 0x21:
                    var op21Value1 = _reader.ReadInt16Le();
                    var op21Value2 = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x21, Value: {op21Value1}, {op21Value2}");
                    break;
                case 0x22:
                    var op22Value1 = _reader.ReadInt16Le();
                    var op22Value2 = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x22, Value: {op22Value1}, {op22Value2}");
                    break;
                case 0x23:
                    var op23Value1 = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x23, Value: {op23Value1}");
                    break;
                case 0x24:
                    // I don't know
                    fullCommands.Add($"Op 0x24");
                    break;
                case 0x25:
                    // Se Play?
                    var op25Value = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x25, Value: {op25Value}");
                    break;
                case 0x26:
                    fullCommands.Add($"Op 0x26");
                    break;
                case 0x27:
                    var op27Length = _reader.ReadInt16Le();
                    var op27Text = _reader.ReadBytesAsCString(op27Length);
                    fullCommands.Add($"Op 0x27, Length: {op27Length}, Text: {op27Text}");
                    break;
                case 0x28:
                    var op28Value1 = _reader.ReadInt16Le();
                    var op28Value2 = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x28, Value: {op28Value1}, {op28Value2}");
                    break;
                case 0x29:
                    // Variable?
                    var op29Value1 = _reader.ReadInt16Le();
                    var op29Value2 = _reader.ReadInt16Le();
                    var op29Value3 = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x29, Value: {op29Value1}, {op29Value2}, {op29Value3}");
                    break;
                case 0x2B:
                    var op2BValue1 = _reader.ReadInt16Le();
                    var op2BValue2 = _reader.ReadInt16Le();
                    var op2BValue3 = _reader.ReadInt16Le();
                    var op2BValue4 = _reader.ReadInt32Le();
                    fullCommands.Add($"Op 0x2B, Value: {op2BValue1}, {op2BValue2}, {op2BValue3}, {op2BValue4}");
                    break;
                case 0x2C:
                    var op2CValue1 = _reader.ReadInt16Le();
                    var op2CValue2 = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x2C, Value: {op2CValue1}, {op2CValue2}");
                    break;
                case 0x2D:
                    var op2DValue1 = _reader.ReadInt16Le();
                    var op2DValue2 = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x2D, Value: {op2DValue1}, {op2DValue2}");
                    break;
                case 0x2E:
                    var op02EValue1 = _reader.ReadInt16Le();
                    var op02EValue2 = _reader.ReadInt16Le();
                    fullCommands.Add($"Op 0x2E, Value: {op02EValue1}, {op02EValue2}");
                    break;
                case 0x2F:
                    var op2FLength = _reader.ReadInt16Le();
                    var op2FText = _reader.ReadBytesAsShiftJisString(op2FLength);
                    fullCommands.Add($"Op 0x2F, Length: {op2FLength}, Text: {op2FText}");
                    texts.Add(op2FText);
                    // Console.WriteLine(op2FText);
                    break;
                case 0x30:
                    var op30Value1 = _reader.ReadInt16Le();
                    var op30Value2 = _reader.ReadInt16Le();
                    var op30Value3 = _reader.ReadInt16Le();
                    var op30Value4 = _reader.ReadInt32Le();
                    fullCommands.Add($"Op 0x30, Value: {op30Value1}, {op30Value2}, {op30Value3}, {op30Value4}");
                    break;
                case 0x31:
                    fullCommands.Add("Op 0x31");
                    break;
                case 0x32:
                    // shit
                    var op32Value = _reader.ReadInt32Le();
                    fullCommands.Add($"Op 0x32, Value: {op32Value}");
                    break;
                default:
                    // pass
                    throw new Exception($"Unknown OpCode: {opCode:X2}, Position: {(_reader.Now() - 2):X2}");
            }
        }
        
        Texts = texts.ToArray();
        FullCommands = fullCommands.ToArray();
    }
}