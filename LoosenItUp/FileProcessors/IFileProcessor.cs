using LoosenItUp.Dtos;

namespace LoosenItUp.FileProcessors
{
    public interface IFileProcessor
    {
        ResultDto Process(string filename);
    }
}
