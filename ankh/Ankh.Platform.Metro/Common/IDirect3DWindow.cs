using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

namespace Ankh.Platform.Metro
{
    public interface IDirect3DWindow
    {
        DXGI.SwapChain1 CreateSwapChain(D3D11.Device1 device, ref DXGI.SwapChainDescription1 description);

        Rectangle<double> Bounds { get; }
    }
}
