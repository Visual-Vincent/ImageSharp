// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Dithering;

namespace SixLabors.ImageSharp.Processing.Processors.Quantization
{
    /// <summary>
    /// Allows the quantization of images pixels using Xiaolin Wu's Color Quantizer <see href="http://www.ece.mcmaster.ca/~xwu/cq.c"/>
    /// <para>
    /// By default the quantizer uses <see cref="KnownDitherings.FloydSteinberg"/> dithering and a color palette of a maximum length of <value>255</value>
    /// </para>
    /// </summary>
    public class WuQuantizer : IQuantizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WuQuantizer"/> class.
        /// </summary>
        public WuQuantizer()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WuQuantizer"/> class.
        /// </summary>
        /// <param name="maxColors">The maximum number of colors to hold in the color palette</param>
        public WuQuantizer(int maxColors)
            : this(GetDiffuser(true), maxColors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WuQuantizer"/> class.
        /// </summary>
        /// <param name="dither">Whether to apply dithering to the output image</param>
        public WuQuantizer(bool dither)
            : this(GetDiffuser(dither), QuantizerConstants.MaxColors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WuQuantizer"/> class.
        /// </summary>
        /// <param name="diffuser">The dithering algorithm, if any, to apply to the output image</param>
        public WuQuantizer(IDither diffuser)
            : this(diffuser, QuantizerConstants.MaxColors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WuQuantizer"/> class.
        /// </summary>
        /// <param name="dither">The dithering algorithm, if any, to apply to the output image</param>
        /// <param name="maxColors">The maximum number of colors to hold in the color palette</param>
        public WuQuantizer(IDither dither, int maxColors)
        {
            this.Dither = dither;
            this.MaxColors = maxColors.Clamp(QuantizerConstants.MinColors, QuantizerConstants.MaxColors);
        }

        /// <inheritdoc />
        public IDither Dither { get; }

        /// <summary>
        /// Gets the maximum number of colors to hold in the color palette.
        /// </summary>
        public int MaxColors { get; }

        /// <inheritdoc />
        public IFrameQuantizer<TPixel> CreateFrameQuantizer<TPixel>(Configuration configuration)
            where TPixel : struct, IPixel<TPixel>
        {
            Guard.NotNull(configuration, nameof(configuration));
            return new WuFrameQuantizer<TPixel>(configuration, this);
        }

        /// <inheritdoc/>
        public IFrameQuantizer<TPixel> CreateFrameQuantizer<TPixel>(Configuration configuration, int maxColors)
            where TPixel : struct, IPixel<TPixel>
        {
            Guard.NotNull(configuration, nameof(configuration));
            maxColors = maxColors.Clamp(QuantizerConstants.MinColors, QuantizerConstants.MaxColors);
            return new WuFrameQuantizer<TPixel>(configuration, this, maxColors);
        }

        private static IDither GetDiffuser(bool dither) => dither ? KnownDitherings.FloydSteinberg : null;
    }
}
