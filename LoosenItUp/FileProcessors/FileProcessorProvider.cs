﻿using LoosenItUp.Factories;

namespace LoosenItUp.FileProcessors
{
    public class FileProcessorProvider
    {
        public static IFileProcessor GetFileProcessor(string filename)
        {
            if (filename.EndsWith(".csv"))
            {
                return ObjectFactory.Create<AggreggateCsvFileProcessor>();
            }
            else
            {
                if (filename.Contains("bossman"))
                {
                    return ObjectFactory.Create<BossmanTxtFileProcessor>();
                }
                else
                {
                    return ObjectFactory.Create<CamdenTxtFileProcessor>();
                }
            }
        }
    }
}
