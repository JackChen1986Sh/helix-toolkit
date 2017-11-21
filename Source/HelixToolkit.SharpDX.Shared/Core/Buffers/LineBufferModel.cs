﻿#if !NETFX_CORE
namespace HelixToolkit.Wpf.SharpDX.Core
#else
namespace HelixToolkit.UWP.Core
#endif
{
    using global::SharpDX.Direct3D;
    using global::SharpDX.Direct3D11;
    using Utilities;
    /// <summary>
    /// Line Geometry Buffer Model. Used for line rendering
    /// </summary>
    /// <typeparam name="VertexStruct"></typeparam>
    public class LineGeometryBufferModel<VertexStruct> : GeometryBufferModel where VertexStruct : struct
    {
        public delegate VertexStruct[] BuildVertexArrayHandler(LineGeometry3D geometry);
        /// <summary>
        /// Create VertexStruct[] from geometry position, colors etc.
        /// </summary>
        public BuildVertexArrayHandler OnBuildVertexArray;

        public LineGeometryBufferModel(int structSize) : base(PrimitiveTopology.LineList,
            new ImmutableBufferProxy<VertexStruct>(structSize, BindFlags.VertexBuffer), new ImmutableBufferProxy<int>(sizeof(int), BindFlags.VertexBuffer))
        {
            OnCreateVertexBuffer = (context, buffer, geometry) =>
            {
                // -- set geometry if given
                if (geometry != null && geometry.Positions != null && OnBuildVertexArray != null)
                {
                    // --- get geometry
                    var mesh = geometry as LineGeometry3D;
                    var data = OnBuildVertexArray(mesh);
                    (buffer as IBufferProxy<VertexStruct>).CreateBufferFromDataArray(context.Device, data, geometry.Positions.Count);
                }
                else
                {
                    buffer.Dispose();
                }
            };
            OnCreateIndexBuffer = (context, buffer, geometry) =>
            {
                if (geometry.Indices != null)
                {
                    (buffer as IBufferProxy<int>).CreateBufferFromDataArray(context.Device, geometry.Indices);
                }
                else
                {
                    buffer.Dispose();
                }
            };
        }
    }
}
