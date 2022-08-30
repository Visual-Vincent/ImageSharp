// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

using System.IO;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

using Xunit;
using Xunit.Abstractions;

namespace SixLabors.ImageSharp.Tests.ProfilingBenchmarks
{
    public class LoadResizeSaveProfilingBenchmarks : MeasureFixture
    {
        public LoadResizeSaveProfilingBenchmarks(ITestOutputHelper output)
            : base(output)
        {
        }

        [Theory(Skip = ProfilingSetup.SkipProfilingTests)]
        [InlineData(TestImages.Jpeg.Baseline.Jpeg420Exif)]
        public void LoadResizeSave(string imagePath)
        {
            var configuration = Configuration.CreateDefaultInstance();
            configuration.MaxDegreeOfParallelism = 1;

            DecoderOptions options = new()
            {
                Configuration = configuration
            };

            byte[] imageBytes = TestFile.Create(imagePath).Bytes;

            using var ms = new MemoryStream();
            this.Measure(
                30,
                () =>
                    {
                        using (var image = Image.Load(options, imageBytes))
                        {
                            image.Mutate(x => x.Resize(image.Size() / 4));
                            image.SaveAsJpeg(ms);
                        }

                        ms.Seek(0, SeekOrigin.Begin);
                    });
        }
    }
}
