using Yukami.Extension;

namespace Yukami.Lib;

public struct Metadata
{
    public string Name;
    public int Offset;
}

public struct Content
{
    public string Name;
    public byte[] Data;
}

public class SCRSplitter
{
    private const byte Key = 0xA5;
    private BinaryReader _reader;
    private Metadata[] _metadata;
    private int _startOffset;
    public Content[] Contents;
    
    public SCRSplitter(FileStream reader)
    {
        _reader = new BinaryReader(reader);
        
        Decrypt();

        ParseMetadata();
        DumpFiles();
    }

    private void Decrypt()
    {
        _reader.GoTo(0x10);
        
        _reader = _reader.XorFromNowToEnd(Key);
    }

    private void ParseMetadata()
    {
        _reader.GoTo(0);

        var header = _reader.ReadBytes(4);
        var fileCount = _reader.ReadInt32Le();
        _startOffset = _reader.ReadInt32Le();
        _reader.Skip(4);
        
        _metadata = new Metadata[fileCount];
        
        for (var i = 0; i < fileCount; i++)
        {
            _metadata[i].Name = _reader.ReadBytesAsCString(12);
            _metadata[i].Offset = _reader.ReadInt32Le();
        }
        
        Contents = new Content[fileCount];
    }

    private void DumpFiles()
    {
        _reader.GoTo(_startOffset);

        for (var i = 0; i < _metadata.Length; i++)
        {
            _reader.GoTo(_startOffset + _metadata[i].Offset);
            var fileLength = i == _metadata.Length - 1
                ? (int) (_reader.BaseStream.Length - _reader.BaseStream.Position)
                : _metadata[i + 1].Offset - _metadata[i].Offset;
            var data = _reader.ReadBytes(fileLength);
            Contents[i] = new Content
            {
                Name = _metadata[i].Name,
                Data = data
            };
        }
    }

}