using System;
using ManagedJupiterHost;
using System.Runtime.InteropServices;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
#if !TARGET_MOBILECORE
using Microsoft.Interop.D2D;
#else
    using Microsoft.Interop.D2D.WinRT;
#endif
using Windows.Graphics.Display;
using Windows.UI.Core;
using System.Threading;

namespace JupiterSUnitDrts.DXInterop
{
    #region Xaml native  interfaces for SIS/VSIS/SE etc.
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct tagPOINT
    {
        public int x;
        public int y;
    }

    public enum D3D_DRIVER_TYPE
    {

        /// D3D_DRIVER_TYPE_UNKNOWN -> 0
        D3D_DRIVER_TYPE_UNKNOWN = 0,

        /// D3D_DRIVER_TYPE_HARDWARE -> (D3D_DRIVER_TYPE_UNKNOWN+1)
        D3D_DRIVER_TYPE_HARDWARE = (D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_UNKNOWN + 1),

        /// D3D_DRIVER_TYPE_REFERENCE -> (D3D_DRIVER_TYPE_HARDWARE+1)
        D3D_DRIVER_TYPE_REFERENCE = (D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE + 1),

        /// D3D_DRIVER_TYPE_NULL -> (D3D_DRIVER_TYPE_REFERENCE+1)
        D3D_DRIVER_TYPE_NULL = (D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_REFERENCE + 1),

        /// D3D_DRIVER_TYPE_SOFTWARE -> (D3D_DRIVER_TYPE_NULL+1)
        D3D_DRIVER_TYPE_SOFTWARE = (D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_NULL + 1),

        /// D3D_DRIVER_TYPE_WARP -> (D3D_DRIVER_TYPE_SOFTWARE+1)
        D3D_DRIVER_TYPE_WARP = (D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_SOFTWARE + 1),
    }

    [ComImport, Guid("F2E9EDC1-D307-4525-9886-0FAFAA44163C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ISurfaceImageSourceNative
    {
        [PreserveSig]
        int SetDevice([In, MarshalAs(UnmanagedType.Interface)] IDXGIDevice pDevice);
        [PreserveSig]
        int BeginDraw([In] tagRECT updateRect, [MarshalAs(UnmanagedType.Interface)] out IDXGISurface pSurface, out tagPOINT Offset);
        [PreserveSig]
        int EndDraw();
    }

    [ComImport, Guid("54298223-41e1-4a41-9c08-02e8256864a1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ISurfaceImageSourceNativeWithD2D
    {
        [PreserveSig]
        int SetDevice([In, MarshalAs(UnmanagedType.IUnknown)] Object pDevice);
        [PreserveSig]
        int BeginDraw(IntPtr updateRect, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out Object pSurface, out tagPOINT Offset);
        [PreserveSig]
        int EndDraw();
        [PreserveSig]
        int SuspendDraw();
        [PreserveSig]
        int ResumeDraw();
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("E9550983-360B-4F53-B391-AFD695078691")]
    public interface IVirtualSurfaceImageSourceNative : ISurfaceImageSourceNative
    {
        [PreserveSig]
        int SetDevice([In, MarshalAs(UnmanagedType.Interface)] IDXGIDevice pDevice);
        [PreserveSig]
        int BeginDraw([In] tagRECT updateRect, [MarshalAs(UnmanagedType.Interface)] out IDXGISurface pSurface, out tagPOINT Offset);
        [PreserveSig]
        int EndDraw();
        [PreserveSig]
        int Invalidate([In] tagRECT updateRect);
        [PreserveSig]
        int GetUpdateRectCount(out uint pCount);
        [PreserveSig]
        int GetUpdateRects(out tagRECT pUpdates, [In] uint Count);
        [PreserveSig]
        int GetVisibleBounds(out tagRECT pBounds);
        [PreserveSig]
        int RegisterForUpdatesNeeded([In, MarshalAs(UnmanagedType.Interface)] IVirtualSurfaceUpdatesCallbackNative pCallback);
        [PreserveSig]
        int Resize([In] int newWidth, [In] int newHeight);
    }

    [ComImport, Guid("DBF2E947-8E6C-4254-9EEE-7738F71386C9"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVirtualSurfaceUpdatesCallbackNative
    {
        [PreserveSig]
        void UpdatesNeeded();
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("43bebd4e-add5-4035-8f85-5608d08e9dc9")]
    public interface ISwapChainBackgroundPanelNative
    {
        [PreserveSig]
        int SetSwapChain([In, MarshalAs(UnmanagedType.Interface)] IDXGISwapChain pSwapChain);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("F92F19D2-3ADE-45A6-A20C-F6F1EA90554B")]
    public interface ISwapChainPanelNative
    {
        [PreserveSig]
        int SetSwapChain([In, MarshalAs(UnmanagedType.Interface)] IDXGISwapChain pSwapChain);
    }

    #endregion Xaml native  interfaces



    public class SurfaceImageSourceInterop
    {
        protected ISurfaceImageSourceNative SISNative { get; set; }
        public SurfaceImageSource SIS { get; set; }
        protected IDXGIDevice DXGIDevice { get; set; }

        protected SurfaceImageSourceInterop()
        {
        }

        public SurfaceImageSourceInterop(int width, int height)
        {
            this.SIS = new SurfaceImageSource(width, height);
            this.SISNative = GetSurfaceImageSourceNative(this.SIS);
        }

        public SurfaceImageSourceInterop(SurfaceImageSource sis)
        {
            this.SIS = sis;
            this.SISNative = GetSurfaceImageSourceNative(this.SIS);
        }

        public static implicit operator SurfaceImageSource(SurfaceImageSourceInterop sisInterop)
        {
            return sisInterop.SIS;
        }

        public static implicit operator SurfaceImageSourceInterop(SurfaceImageSource sis)
        {
            return new SurfaceImageSourceInterop(sis);
        }

        public void SetDevice(IDXGIDevice dxgiDevice)
        {
            int hResult = this.SISNative.SetDevice(dxgiDevice);

            if (hResult != 0)
                throw new InvalidOperationException("SetDevice Failed with error code" + hResult.ToString("X"));

            this.DXGIDevice = dxgiDevice;
        }

        public bool BeginDraw(Rect updateRect, out IDXGISurface scratchSurface, out Point offset)
        {
            tagPOINT offsetTagPoint;
            offset = new Point();
            tagRECT _updateRect = new tagRECT() { left = (int)updateRect.X, top = (int)updateRect.Y, right = (int)(updateRect.X + updateRect.Width), bottom = (int)(updateRect.Y + updateRect.Height) };
            int hResult = this.SISNative.BeginDraw(_updateRect, out scratchSurface, out offsetTagPoint);

            if (hResult != 0)
            {
                if (hResult == unchecked((int)0x887A0005)/* DXGI_ERROR_DEVICE_REMOVED */ || hResult == unchecked((int)0x887A0007) /* DXGI_ERROR_DEVICE_RESET */)
                {
                    this.DXGIDevice = null;
                }
                else
                {
                    throw new InvalidOperationException("BeginDraw Failed with error code" + hResult.ToString("X"));
                }
                return false;
            }

            offset.X = offsetTagPoint.x;
            offset.Y = offsetTagPoint.y;
            return true;
        }

        public void EndDraw()
        {
            int hResult = this.SISNative.EndDraw();

            if (hResult != 0)
                throw new InvalidOperationException("EndDraw Failed with error code" + hResult.ToString("X"));
        }

        public static ISurfaceImageSourceNative GetSurfaceImageSourceNative(Windows.UI.Xaml.Media.Imaging.SurfaceImageSource surfaceImageSource)
        {
            ISurfaceImageSourceNative surfaceImageSourceNative = (ISurfaceImageSourceNative)(Object)surfaceImageSource;
            return surfaceImageSourceNative;
        }

        public void UpdateSurface(Rect updateRect, float[] color)
        {
            IDXGISurface scratchSurface = null;
            Point offset;

            if (this.DXGIDevice == null)
            {
                ID3D11Device d3d11Device;
                DXInteropHelper.InitializeD3D(D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE, out d3d11Device, D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_BGRA_SUPPORT);
                IDXGIDevice dxgiDevice = DXInteropHelper.GetDXGIDevice(d3d11Device);
                SetDevice(dxgiDevice);
            }

            // devide the update into regions under device capacity
            // TDOO: should probably get device capacity?
            int MAXWIDTH = 2048;
            int MAXHEIGHT = 2048;
            Rect rect = new Rect() { X = 0, Y = 0, Height = 0, Width = 0 };
            float updateRectRight = (float)(updateRect.X + updateRect.Width);
            float updateRectBottom = (float)(updateRect.Y + updateRect.Height);

            for (rect.X = updateRect.X; rect.X < updateRectRight; rect.X += MAXWIDTH)
            {
                for (rect.Y = updateRect.Y; rect.Y < updateRectBottom; rect.Y += MAXHEIGHT)
                {
                    rect.Width = (rect.X + MAXWIDTH) > updateRectRight ? updateRectRight - rect.X : MAXWIDTH;
                    rect.Height = (rect.Y + MAXHEIGHT) > updateRectBottom ? updateRectBottom - rect.Y : MAXHEIGHT;

                    if (this.BeginDraw(rect, out scratchSurface, out offset))
                    {
                        DXInteropHelper.Render(scratchSurface, this.DXGIDevice, color);
                        this.EndDraw();
                    }
                    else
                    {
                        UpdateSurface(updateRect, color);
                        return;
                    }
                }
            }
        }
    }

    public class SurfaceImageSourceWithD2DInterop
    {
        protected ISurfaceImageSourceNativeWithD2D SISNative { get; set; }
        public SurfaceImageSource SIS { get; set; }
        protected Object Device { get; set; }
        protected bool IsBatching;
        private DeviceContext d2dDeviceContext;
        protected int Width, Height;
        protected SurfaceImageSourceWithD2DInterop()
        {
        }

        public SurfaceImageSourceWithD2DInterop(int width, int height, bool isbatching)
        {
            Width = width;
            Height = height;
            this.SIS = new SurfaceImageSource(width, height);
            IsBatching = isbatching;
            this.SISNative = GetSurfaceImageSourceNativeWithD2D(this.SIS);
            this.Device = null;
        }

        public SurfaceImageSourceWithD2DInterop(SurfaceImageSource sis)
        {
            this.SIS = sis;
            this.SISNative = GetSurfaceImageSourceNativeWithD2D(this.SIS);
        }

        public static implicit operator SurfaceImageSource(SurfaceImageSourceWithD2DInterop sisInterop)
        {
            return sisInterop.SIS;
        }

        public static implicit operator SurfaceImageSourceWithD2DInterop(SurfaceImageSource sis)
        {
            return new SurfaceImageSourceWithD2DInterop(sis);
        }

        public void SetDevice(Object device)
        {
            int hResult = this.SISNative.SetDevice(device);

            if (hResult != 0)
                throw new InvalidOperationException("SetDevice Failed with error code" + hResult.ToString("X"));

            this.Device = device;
        }

        public bool BeginDrawWithD2D(Rect updateRect, ref Guid refiid, out Object scratchSurface, out Point offset)
        {
            tagPOINT offsetTagPoint;
            offset = new Point();
            tagRECT _updateRect = new tagRECT()
            {
                left = (int)updateRect.X,
                top = (int)updateRect.Y,
                right = (int)(updateRect.X + updateRect.Width),
                bottom = (int)(updateRect.Y + updateRect.Height)
            };

            IntPtr rect = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(tagRECT)));
            Marshal.StructureToPtr(_updateRect, rect, false);
            int hResult = this.SISNative.BeginDraw(rect, refiid, out scratchSurface, out offsetTagPoint);

            if (hResult != 0)
            {
                if (hResult == unchecked((int)0x802b0020)/* E_SURFACE_CONTENTS_LOST */ || hResult == unchecked((int)0x887A0005)/* DXGI_ERROR_DEVICE_REMOVED */ || hResult == unchecked((int)0x887A0007) /* DXGI_ERROR_DEVICE_RESET */)
                {
                    this.Device = null;
                }
                else
                {
                    throw new InvalidOperationException("BeginDrawWithD2D Failed with error code" + hResult.ToString("X"));
                }
                return false;
            }

            offset.X = offsetTagPoint.x;
            offset.Y = offsetTagPoint.y;
            return true;
        }

        public void SuspendDraw()
        {
            int hResult = this.SISNative.SuspendDraw();
            if (hResult != 0)
            {
                if (hResult == unchecked((int)0x802b0020)/* E_SURFACE_CONTENTS_LOST */ || hResult == unchecked((int)0x887A0005)/* DXGI_ERROR_DEVICE_REMOVED */ || hResult == unchecked((int)0x887A0007) /* DXGI_ERROR_DEVICE_RESET */)
                {
                    this.Device = null;
                }
                else
                {
                    throw new InvalidOperationException("SuspendDraw Failed with error code" + hResult.ToString("X"));
                }
            }
        }

        public void ResumeDraw()
        {
            int hResult = this.SISNative.ResumeDraw();

            if (hResult != 0)
            {
                if (hResult == unchecked((int)0x802b0020)/* E_SURFACE_CONTENTS_LOST */ || hResult == unchecked((int)0x887A0005)/* DXGI_ERROR_DEVICE_REMOVED */ || hResult == unchecked((int)0x887A0007) /* DXGI_ERROR_DEVICE_RESET */)
                {
                    this.Device = null;
                }
                else
                {
                    throw new InvalidOperationException("ResumeDraw Failed with error code" + hResult.ToString("X"));
                }
            }
        }

        public void EndDrawWithD2D()
        {
            int hResult = this.SISNative.EndDraw();

            if (hResult != 0)
                throw new InvalidOperationException("EndDrawWithD2D Failed with error code" + hResult.ToString("X"));
        }

        public void EndDraw()
        {
            int hResult = this.SISNative.EndDraw();

            if (hResult != 0)
                throw new InvalidOperationException("EndDraw Failed with error code" + hResult.ToString("X"));
        }

        public static ISurfaceImageSourceNativeWithD2D GetSurfaceImageSourceNativeWithD2D(Windows.UI.Xaml.Media.Imaging.SurfaceImageSource surfaceImageSource)
        {
            ISurfaceImageSourceNativeWithD2D surfaceImageSourceNative = (ISurfaceImageSourceNativeWithD2D)surfaceImageSource;
            return surfaceImageSourceNative;
        }

        // This is to used for first update. First update always needs to full rect.
        public void UpdateSurfaceWithD2D()
        {
            Rect fullRect = new Rect(0, 0, Width, Height);
            // first update with full rect
            this.UpdateSurfaceWithD2D(fullRect, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
        }

        //return wherther first begindraw succeeded or not
        public bool UpdateSurfaceWithD2D(Rect updateRect, float[] color, bool suspendAfterDraw = false)
        {
            Object updatedObject;
            Point offset;
            if (this.Device == null)
            {
                ID3D11Device d3d11Device;
                DXInteropHelper.InitializeD3D(D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE, out d3d11Device, D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_BGRA_SUPPORT);
                IDXGIDevice dxgiDevice = DXInteropHelper.GetDXGIDevice(d3d11Device);
                if (!this.IsBatching)
                {
                    SetDevice(dxgiDevice);
                    DXInteropHelper.InitializeD2D(d3d11Device, out d2dDeviceContext);
                }
                else
                {
                    Device d2dDevice = DXInteropHelper.CreateD2DDevice(d3d11Device);
                    SetDevice(d2dDevice);
                }
            }

            Guid guid = !IsBatching ? new Guid("CAFCB56C-6AC3-4889-BF47-9E23BBD260EC") : new Guid("e8f7fe7a-191c-466d-ad95-975678bda998");
            bool isSucceeded = this.BeginDrawWithD2D(updateRect, ref guid, out updatedObject, out offset);
            if (!isSucceeded)
            {
                UpdateSurfaceWithD2D(updateRect, color, suspendAfterDraw);
                return isSucceeded;
            }

            if (!IsBatching)
            {
                DXInteropHelper.Render((IDXGISurface)updatedObject, (IDXGIDevice)this.Device, color);
                //DXInteropHelper.RenderRectangleWithD2D((IDXGISurface)updatedObject, d2dDeviceContext, color, new Rect(offset.X, offset.Y, updateRect.Width, updateRect.Height));
            }
            else
            {
#if !TARGET_MOBILECORE
                ColorF? colorF = new ColorF(color[0], color[1], color[2], color[3]);
                ((DeviceContext)updatedObject).Clear(ref colorF);
#else
                ColorF colorF = new ColorF(color[0], color[1], color[2], color[3]);
                ((DeviceContext)updatedObject).Clear(colorF);
#endif
            }

            if (suspendAfterDraw)
            {
                this.SuspendDraw();
            }
            else
            {
                this.EndDraw();
            }
            return isSucceeded;
        }

        public void UpdateSurfaceWithMultipleBeginDraws(int tileSize = 50)
        {
            this.UpdateSurfaceWithD2D();

            int count = 0;
            for (int x = 0; x < Width; x += tileSize)
            {
                for (int y = 0; y < Height; y += tileSize)
                {
                    Rect updateRect = new Rect(x, y, Math.Min(tileSize, Width - x), Math.Min(tileSize, Height - y));
                    float[] updateColor = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
                    switch (count % 4)
                    {
                        case 0:
                            updateColor = new float[] { 0.0f, 0.0f, 1.0f, 1.0f };
                            break;
                        case 1:
                            updateColor = new float[] { 0.0f, 1.0f, 0.0f, 1.0f };
                            break;
                        case 2:
                            updateColor = new float[] { 1.0f, 0.0f, 0.0f, 1.0f };
                            break;
                        default:
                            break;
                    }
                    count++;
                    this.UpdateSurfaceWithD2D(updateRect, updateColor, true);
                }
            }

            this.EndDraw();
        }
    }

    public class VirtualSurfaceImageSourceInterop : SurfaceImageSourceInterop
    {
        // TODO: factor this out of this class
        public int CountOfUpdatesNeededNotifications { get; set; }

        public VirtualSurfaceImageSourceInterop(int width, int height)
        {
            this.SIS = new VirtualSurfaceImageSource(width, height);
            this.SISNative = GetVirtualSurfaceImageSourceNative((VirtualSurfaceImageSource)this.SIS);
        }

        public void Invalidate(Rect updateRect)
        {
            tagRECT _updateRect = new tagRECT() { left = (int)updateRect.X, top = (int)updateRect.Y, right = (int)(updateRect.X + updateRect.Width), bottom = (int)(updateRect.Y + updateRect.Height) };

            int hResult = ((IVirtualSurfaceImageSourceNative)this.SISNative).Invalidate(_updateRect);

            if (hResult != 0)
                throw new InvalidOperationException("Invalidate Failed with error code" + hResult.ToString("X"));
        }

        public VirtualSurfaceImageSourceInterop(VirtualSurfaceImageSource vsis)
        {
            this.SIS = vsis;
            this.SISNative = GetVirtualSurfaceImageSourceNative((VirtualSurfaceImageSource)this.SIS);
        }

        public static implicit operator VirtualSurfaceImageSourceInterop(VirtualSurfaceImageSource vsis)
        {
            return new VirtualSurfaceImageSourceInterop(vsis);
        }

        public int GetUpdateRectCount()
        {
            uint count;
            int hResult = ((IVirtualSurfaceImageSourceNative)this.SISNative).GetUpdateRectCount(out count);

            if (hResult != 0)
                throw new InvalidOperationException("GetUpdateRectCount Failed with error code" + hResult.ToString("X"));

            return (int)count;
        }

        public List<Rect> GetUpdateRects(int count)
        {
            List<Rect> updateRects = new List<Rect>();
            if (count > 0)
            {
                tagRECT[] _updateRects = new tagRECT[count];
                int hResult = ((IVirtualSurfaceImageSourceNative)this.SISNative).GetUpdateRects(out _updateRects[0], (uint)count);

                if (hResult != 0)
                    throw new InvalidOperationException("GetUpdateRectCount Failed with error code" + hResult.ToString("X"));

                for (int i = 0; i < count; ++i)
                {
                    updateRects.Add(new Rect()
                    {
                        X = _updateRects[i].left,
                        Y = _updateRects[i].top,
                        Width = _updateRects[i].right - _updateRects[i].left,
                        Height = _updateRects[i].bottom - _updateRects[i].top
                    });
                }
            }
            return updateRects;
        }

        public Rect GetVisibleBounds()
        {
            tagRECT _updateRect = new tagRECT();
            int hResult = ((IVirtualSurfaceImageSourceNative)this.SISNative).GetVisibleBounds(out _updateRect);

            if (hResult != 0)
                throw new InvalidOperationException("GetVisibleBounds Failed with error code" + hResult.ToString("X"));

            return new Rect()
            {
                X = _updateRect.left,
                Y = _updateRect.top,
                Width = _updateRect.right - _updateRect.left,
                Height = _updateRect.bottom - _updateRect.top,
            };
        }

        public void RegisterForUpdatesNeeded(IVirtualSurfaceUpdatesCallbackNative pCallback)
        {
            int hResult = ((IVirtualSurfaceImageSourceNative)this.SISNative).RegisterForUpdatesNeeded(pCallback);

            if (hResult != 0)
                throw new InvalidOperationException("RegisterForUpdatesNeeded Failed with error code" + hResult.ToString("X"));
        }

        public void Resize(int newWidth, int newHeight)
        {
            int hResult = ((IVirtualSurfaceImageSourceNative)this.SISNative).Resize(newWidth, newHeight);

            if (hResult != 0)
                throw new InvalidOperationException("Resize Failed with error code" + hResult.ToString("X"));
        }

        public void UpdateSurface(float[] color)
        {
            int updateRectCount = this.GetUpdateRectCount();
            List<Rect> updateRects = this.GetUpdateRects(updateRectCount);

            foreach (Rect updateRect in updateRects)
            {
                this.UpdateSurface(updateRect, color);
            }
        }

        public static IVirtualSurfaceImageSourceNative GetVirtualSurfaceImageSourceNative(Windows.UI.Xaml.Media.Imaging.VirtualSurfaceImageSource virtualSurfaceImageSource)
        {
            IVirtualSurfaceImageSourceNative virtualSurfaceImageSourceNative = (IVirtualSurfaceImageSourceNative)(Object)virtualSurfaceImageSource;
            return virtualSurfaceImageSourceNative;
        }

    }

    public class VirtualSurfaceImageSourceWithD2DInterop : SurfaceImageSourceWithD2DInterop
    {
        protected IVirtualSurfaceImageSourceNative VSISNative { get; set; }
        // TODO: factor this out of this class
        public int CountOfUpdatesNeededNotifications { get; set; }

        public VirtualSurfaceImageSourceWithD2DInterop(int width, int height, bool isbatching)
        {
            Width = width;
            Height = height;
            this.SIS = new VirtualSurfaceImageSource(width, height);
            this.VSISNative = GetVirtualSurfaceImageSourceNative((VirtualSurfaceImageSource)this.SIS);
            this.SISNative = GetSurfaceImageSourceNativeWithD2D((VirtualSurfaceImageSource)this.SIS);
            IsBatching = isbatching;
            this.Device = null;
        }

        public void Invalidate(Rect updateRect)
        {
            tagRECT _updateRect = new tagRECT() { left = (int)updateRect.X, top = (int)updateRect.Y, right = (int)(updateRect.X + updateRect.Width), bottom = (int)(updateRect.Y + updateRect.Height) };

            int hResult = this.VSISNative.Invalidate(_updateRect);

            if (hResult != 0)
                throw new InvalidOperationException("Invalidate Failed with error code" + hResult.ToString("X"));
        }

        public VirtualSurfaceImageSourceWithD2DInterop(VirtualSurfaceImageSource vsis)
        {
            this.SIS = vsis;
            this.VSISNative = GetVirtualSurfaceImageSourceNative((VirtualSurfaceImageSource)this.SIS);
            this.SISNative = GetSurfaceImageSourceNativeWithD2D((VirtualSurfaceImageSource)this.SIS);
        }

        public static implicit operator VirtualSurfaceImageSourceWithD2DInterop(VirtualSurfaceImageSource vsis)
        {
            return new VirtualSurfaceImageSourceWithD2DInterop(vsis);
        }

        public int GetUpdateRectCount()
        {
            uint count;
            int hResult = this.VSISNative.GetUpdateRectCount(out count);

            if (hResult != 0)
                throw new InvalidOperationException("GetUpdateRectCount Failed with error code" + hResult.ToString("X"));

            return (int)count;
        }

        public List<Rect> GetUpdateRects(int count)
        {
            List<Rect> updateRects = new List<Rect>();
            if (count > 0)
            {
                tagRECT[] _updateRects = new tagRECT[count];
                int hResult = this.VSISNative.GetUpdateRects(out _updateRects[0], (uint)count);

                if (hResult != 0)
                    throw new InvalidOperationException("GetUpdateRectCount Failed with error code" + hResult.ToString("X"));

                for (int i = 0; i < count; ++i)
                {
                    updateRects.Add(new Rect()
                    {
                        X = _updateRects[i].left,
                        Y = _updateRects[i].top,
                        Width = _updateRects[i].right - _updateRects[i].left,
                        Height = _updateRects[i].bottom - _updateRects[i].top
                    });
                }
            }
            return updateRects;
        }

        public Rect GetVisibleBounds()
        {
            tagRECT _updateRect = new tagRECT();
            int hResult = this.VSISNative.GetVisibleBounds(out _updateRect);

            if (hResult != 0)
                throw new InvalidOperationException("GetVisibleBounds Failed with error code" + hResult.ToString("X"));

            return new Rect()
            {
                X = _updateRect.left,
                Y = _updateRect.top,
                Width = _updateRect.right - _updateRect.left,
                Height = _updateRect.bottom - _updateRect.top,
            };
        }

        public void RegisterForUpdatesNeeded(IVirtualSurfaceUpdatesCallbackNative pCallback)
        {
            int hResult = this.VSISNative.RegisterForUpdatesNeeded(pCallback);

            if (hResult != 0)
                throw new InvalidOperationException("RegisterForUpdatesNeeded Failed with error code" + hResult.ToString("X"));
        }

        public void Resize(int newWidth, int newHeight)
        {
            int hResult = this.VSISNative.Resize(newWidth, newHeight);

            if (hResult != 0)
                throw new InvalidOperationException("Resize Failed with error code" + hResult.ToString("X"));
        }

        public void UpdateSurface(float[] color)
        {
            int updateRectCount = this.GetUpdateRectCount();
            List<Rect> updateRects = this.GetUpdateRects(updateRectCount);

            foreach (Rect updateRect in updateRects)
            {
                this.UpdateSurfaceWithD2D(updateRect, color);
            }
        }

        public static IVirtualSurfaceImageSourceNative GetVirtualSurfaceImageSourceNative(Windows.UI.Xaml.Media.Imaging.VirtualSurfaceImageSource virtualSurfaceImageSource)
        {
            IVirtualSurfaceImageSourceNative virtualSurfaceImageSourceNative = (IVirtualSurfaceImageSourceNative)(Object)virtualSurfaceImageSource;
            return virtualSurfaceImageSourceNative;
        }
    }

    // TODO: the following two classes should be removed in the future
    // TODO: after vsis priority callback change, their behavior become non-deterministic

    // NOTE: VirtualSurfaceUpdatesCallback1/2 are kept here due to exisiting external dependencies.
    // NOTE: please avoid using these classes

    public class VirtualSurfaceUpdatesCallback1 : IVirtualSurfaceUpdatesCallbackNative
    {
        private VirtualSurfaceImageSourceInterop _VirtualSurfaceImageSourceInterop { get; set; }
        public VirtualSurfaceUpdatesCallback1(VirtualSurfaceImageSourceInterop vsisInterop)
        {
            this._VirtualSurfaceImageSourceInterop = vsisInterop;
        }

        public void UpdatesNeeded()
        {

            switch (this._VirtualSurfaceImageSourceInterop.CountOfUpdatesNeededNotifications)
            {
                case 0: //update 1
                    this._VirtualSurfaceImageSourceInterop.UpdateSurface(new float[] { 0f, 0f, 1f, 1.0f }); //Blue
                    break;
                case 1: //update 2
                    this._VirtualSurfaceImageSourceInterop.UpdateSurface(new float[] { 0f, 1f, 0f, 1.0f }); //Green
                    break;
                case 2: // update 3
                    this._VirtualSurfaceImageSourceInterop.UpdateSurface(new float[] { 1f, 0f, 0f, 1.0f }); //Red
                    break;
                default:
                    this._VirtualSurfaceImageSourceInterop.UpdateSurface(new float[] { 0f, 0f, 0f, 1.0f }); //Black
                    break;
            }
            ++this._VirtualSurfaceImageSourceInterop.CountOfUpdatesNeededNotifications;


        }
    }

    public class VirtualSurfaceUpdatesCallback2 : IVirtualSurfaceUpdatesCallbackNative
    {
        private VirtualSurfaceImageSourceInterop _VirtualSurfaceImageSourceInterop { get; set; }
        public VirtualSurfaceUpdatesCallback2(VirtualSurfaceImageSourceInterop vsisInterop)
        {
            this._VirtualSurfaceImageSourceInterop = vsisInterop;
        }

        public void UpdatesNeeded()
        {

            switch (this._VirtualSurfaceImageSourceInterop.CountOfUpdatesNeededNotifications)
            {
                case 0: //update 1
                    this._VirtualSurfaceImageSourceInterop.UpdateSurface(new float[] { 1f, 0f, 1f, 0.5f }); //Pink
                    break;
                case 1: //update 2
                    this._VirtualSurfaceImageSourceInterop.UpdateSurface(new float[] { 1f, 1f, 0f, 0.5f }); //Yellow
                    break;
                case 2: //update 3
                    this._VirtualSurfaceImageSourceInterop.UpdateSurface(new float[] { 0f, 1f, 1f, 0.5f }); //Cyan
                    break;
                default:
                    this._VirtualSurfaceImageSourceInterop.UpdateSurface(new float[] { 0f, 0f, 0f, 0.5f }); //Black
                    break;
            }

            ++this._VirtualSurfaceImageSourceInterop.CountOfUpdatesNeededNotifications;


        }
    }

    public class SwapChainPanelInteropBase
    {
        public IDXGISwapChain DXGISwapChain { get; set; }

        public ISwapChainPanelNative SCPNative { get; protected set; }
        public SwapChainPanel SCP { get; protected set; }

        public ISwapChainBackgroundPanelNative SCBPNative { get; protected set; }
        public SwapChainBackgroundPanel SCBP { get; protected set; }

        protected CoreDispatcher Dispatcher { get; set; }

        private ID3D10Multithread D3DMultiThreadLock { get; set; }

        protected SwapChainPanelInteropBase()
        {
            this.Dispatcher = Windows.UI.Xaml.Window.Current.Dispatcher;
        }

        DeviceContext _d2dDeviceContext = null;
        public DeviceContext D2DDeviceContext
        {
            get
            {
                if (_d2dDeviceContext == null)
                {
                    IDXGIDevice dxgiDevice = this.GetDeviceFromSwapChain();
                    ID3D11Device d3d11Device = DXInteropHelper.GetD3D11Device(dxgiDevice);
                    D3DMultiThreadLock = (ID3D10Multithread)(Object)d3d11Device;
                    DXInteropHelper.InitializeD2D(d3d11Device, out _d2dDeviceContext);
                }

                return _d2dDeviceContext;
            }
            private set
            {
                _d2dDeviceContext = value;
            }
        }

        private List<Bitmap1> swapChainBufferBitmaps = new List<Bitmap1>();
        private ManualResetEvent waiter = new ManualResetEvent(true);
        private DXGI_SWAP_CHAIN_DESC swapChaindesc;

        public void SetSwapChain(IDXGISwapChain dxgiSwapChain)
        {
            int hResult = (this.SCBPNative != null) ? this.SCBPNative.SetSwapChain(dxgiSwapChain) : this.SCPNative.SetSwapChain(dxgiSwapChain);

            if (hResult != 0)
                throw new InvalidOperationException("SetSwapChain Failed with error code" + hResult.ToString("X"));

            this.DXGISwapChain = dxgiSwapChain;

            if (this.DXGISwapChain != null)
            {
                // A new swapchain has been set, which might be created using a different D3DDevice, setting the D2DDeviceContext to null, so that when 
                // this.D2DDeviceContext is needed to draw, a new DeviceContext will be create out of the D3DDevice device used to create the new SwapChain.
                this.D2DDeviceContext = null;
                CacheBuffers();
            }
            else
            {
                swapChainBufferBitmaps.Clear();
                this.D2DDeviceContext = null;
            }
        }

        private void CacheBuffers()
        {
            waiter.WaitOne();

            if (swapChainBufferBitmaps.Count > 0)
            {
                foreach (Bitmap1 bitmap in swapChainBufferBitmaps) bitmap.Dispose();
                swapChainBufferBitmaps.Clear();
            }

            this.DXGISwapChain.GetDesc(out swapChaindesc);
            for (int i = 0; i < swapChaindesc.BufferCount; i++)
            {
                IDXGISurface1 surface = this.GetSurfaceFromSwapChain(i);
                swapChainBufferBitmaps.Add(DXInteropHelper.GetD2DBitmapFromSurface(surface, this.D2DDeviceContext));
                Marshal.ReleaseComObject(surface);
            }
        }

        public void EnsureSwapChain(ID3D11Device d3d11Device)
        {
            //create default swapchain if test does not set one.
            DeviceContext d2dDeviceContext = null;

            if (d3d11Device == null)
            {
                DXInteropHelper.InitializeD3D(D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE, out d3d11Device, D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_BGRA_SUPPORT);
            }

            DXInteropHelper.InitializeD2D(d3d11Device, out d2dDeviceContext);

            int width = (this.SCBPNative != null) ? (int)Windows.UI.Xaml.Window.Current.Bounds.Width : (int)this.SCP.Width;
            int height = (this.SCBPNative != null) ? (int)Windows.UI.Xaml.Window.Current.Bounds.Height : (int)this.SCP.Height;
            IDXGISwapChain swapChain = DXInteropHelper.CreateSwapChainForComposition(d3d11Device, width, height);

            this.SetSwapChain(swapChain);

        }

        private IDXGIDevice1 GetDeviceFromSwapChain()
        {
            Guid dxgiDevice1Guid = new Guid("77DB970F-6276-48BA-BA28-070143B4392C");
            IDXGIDevice1 dxgiDevice = (IDXGIDevice1)Marshal.GetObjectForIUnknown(this.DXGISwapChain.GetDevice(ref dxgiDevice1Guid));
            return dxgiDevice;
        }

        private IDXGISurface1 GetSurfaceFromSwapChain(int buffer)
        {
            IDXGISurface1 surface = null;
            IntPtr surfacePtr = IntPtr.Zero;
            Guid dxgiSurface1Guid = new Guid("4AE63092-6327-4C1B-80AE-BFE12EA32B86");
            this.DXGISwapChain.GetBuffer((uint)buffer, ref dxgiSurface1Guid, ref surfacePtr);

            surface = (IDXGISurface1)Marshal.GetObjectForIUnknown(surfacePtr);

            Marshal.Release(surfacePtr);
            return surface;
        }

        public void ResizeBuffers(int width, int height, bool adjustScale = true)
        {
            waiter.WaitOne();
            foreach (Bitmap1 bitmap in swapChainBufferBitmaps) bitmap.Dispose();

            if (this.SCBPNative != null || !adjustScale)
            {
                this.DXGISwapChain.ResizeBuffers(swapChaindesc.BufferCount, (uint)width, (uint)height, swapChaindesc.BufferDesc.Format, 0);
            }
            else
            {
                this.DXGISwapChain.ResizeBuffers(swapChaindesc.BufferCount, (uint)(width * this.SCP.CompositionScaleX), (uint)(height * this.SCP.CompositionScaleY), swapChaindesc.BufferDesc.Format, 0);

                DXGI_MATRIX_3X2_F inverseScale = new DXGI_MATRIX_3X2_F { _11 = 1 / this.SCP.CompositionScaleX, _22 = 1 / this.SCP.CompositionScaleY };
                ((IDXGISwapChain2)this.DXGISwapChain).SetMatrixTransform(ref inverseScale);
            }

            CacheBuffers();
        }

        public void CopyBuffers(int src, int dest)
        {
            waiter.WaitOne();

#if !TARGET_MOBILECORE
            Point2U? point = null;
            RectU? srcRect = null;
            swapChainBufferBitmaps[dest].CopyFromBitmap(ref point, swapChainBufferBitmaps[src], ref srcRect);
#endif
        }

        private void UpdateSurfaceImpl(Action<IDXGISurface> renderD3DFunc, Action<Bitmap1> renderD2DFunc, bool copyBuffers = false)
        {
            try
            {
                if (this.DXGISwapChain == null)
                {
                    EnsureSwapChain(null);
                }

                IDXGISurface1 surface = null;

                Bitmap1 bitmap = null;
                if (renderD3DFunc != null)
                {
#if !TARGET_MOBILECORE
                    surface = (IDXGISurface1)Marshal.GetObjectForIUnknown(swapChainBufferBitmaps[0].GetSurface());
#else
                    surface = this.GetSurfaceFromSwapChain(0);
#endif
                    renderD3DFunc(surface);
                }
                else
                {
#if !TARGET_MOBILECORE
                    bitmap = swapChainBufferBitmaps[0].GetBitmap1();
#else
                    bitmap = swapChainBufferBitmaps[0];
#endif
                    renderD2DFunc(bitmap);
                }

                waiter.Reset();
                Action presentAction = () =>
                {
                    Present();
                    waiter.Set();
                };

                //Present
                if (DXInteropHelper.HasUIThreadAccess() 
                    || this.Dispatcher == null) /* the dispatcher is null for xamlpresenter case where it is not running on UIThread */
                    presentAction();
                else
                    this.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                        presentAction());

                waiter.WaitOne();

                if (copyBuffers)
                {
                    this.CopyBuffers(1, 0);
                }


                if (renderD3DFunc != null)
                    Marshal.ReleaseComObject(surface);

                if (renderD2DFunc != null)
                    bitmap.Dispose();

            }
            catch (Exception ex)
            {
                int hr = Marshal.GetHRForException(ex);
                if (hr == unchecked((int)0x887A0005)/* DXGI_ERROR_DEVICE_REMOVED */ || hr == unchecked((int)0x887A0007) /* DXGI_ERROR_DEVICE_RESET */)
                {
                    this.DXGISwapChain = null;
                    UpdateSurfaceImpl(renderD3DFunc, renderD2DFunc, copyBuffers);
                }
                else
                {
                    throw new InvalidOperationException("Present Failed with error code" + hr.ToString("X"));
                }
            }
        }

        public void Present()
        {
            D3DMultiThreadLock.Enter();
            this.DXGISwapChain.Present(1, 0);
            D3DMultiThreadLock.Leave();
        }

        public void UpdateSurface(float[] color, bool copyBuffers = false)
        {
            UpdateSurfaceImpl((IDXGISurface surface) =>
            {
                IDXGIDevice1 dxgiDevice = this.GetDeviceFromSwapChain();
                DXInteropHelper.Render(surface, dxgiDevice, color);
            }, null, copyBuffers);
        }

        public void UpdateSurfaceWithD2D(float[] color, bool copyBuffers = false)
        {
            UpdateSurfaceImpl(null, (Bitmap1 bitmap) =>
            {
                DXInteropHelper.RenderWithD2D(bitmap, this.D2DDeviceContext, color);
            }, copyBuffers);
        }

        public void UpdateSurfaceWithD2DClock(float[] color, bool enableDFlip = false, bool copyBuffers = false)
        {
            DXGI_SWAP_CHAIN_DESC swapChainDesc;

            UpdateSurfaceImpl(null, (Bitmap1 bitmap) =>
            {
                this.DXGISwapChain.GetDesc(out swapChainDesc);

                int width = (int)swapChainDesc.BufferDesc.Width;
                int height = (int)swapChainDesc.BufferDesc.Height;
                DXInteropHelper.RenderClockWithD2D(bitmap, this.D2DDeviceContext, color, width, height, enableDFlip);
            }, copyBuffers);
        }


        public void UpdateSurfaceWithD2DTiles(bool copyBuffers = false)
        {
            UpdateSurfaceImpl(null, (Bitmap1 bitmap) =>
            {
                DXInteropHelper.RenderTilesWithD2D(bitmap, this.D2DDeviceContext);
            }, copyBuffers);
        }

        public void UpdateSurfaceWithD2DRectangle(float[] color, Rect rect, bool copyBuffers = false)
        {
            UpdateSurfaceImpl(null, (Bitmap1 bitmap) =>
            {
                DXInteropHelper.RenderRectangleWithD2D(bitmap, this.D2DDeviceContext, color, rect);
            }, copyBuffers);
        }

        public void UpdateSurfaceWithD2DEllipse(Point centerPoint, float radius, float[] color, bool copyBuffers = false)
        {
            UpdateSurfaceImpl(null, (Bitmap1 bitmap) =>
            {
                DXInteropHelper.RenderEllipseWithD2D(bitmap, this.D2DDeviceContext, centerPoint, radius, color);
            }, copyBuffers);
        }

    }

    public class SwapChainPanelInterop : SwapChainPanelInteropBase
    {
        public SwapChainPanelInterop(SwapChainPanel scp)
        {
            SCP = scp;
            SCPNative = GetSwapChainPanelNative(SCP);
        }

        public static implicit operator SwapChainPanel(SwapChainPanelInterop scpInterop)
        {
            return scpInterop.SCP;
        }

        public static implicit operator SwapChainPanelInterop(SwapChainPanel scp)
        {
            return new SwapChainPanelInterop(scp);
        }

        public static ISwapChainPanelNative GetSwapChainPanelNative(SwapChainPanel scp)
        {
            ISwapChainPanelNative scpNative = (ISwapChainPanelNative)(Object)scp;
            return scpNative;
        }
    }

    public class SwapChainBackgroundPanelInterop : SwapChainPanelInteropBase
    {

        public SwapChainBackgroundPanelInterop(SwapChainBackgroundPanel scbp)
        {
            this.SCBP = scbp;
            this.SCBPNative = GetSwapChainBackgroundPanelNative(this.SCBP);
        }

        public static ISwapChainBackgroundPanelNative GetSwapChainBackgroundPanelNative(SwapChainBackgroundPanel scbp)
        {
            ISwapChainBackgroundPanelNative scbpNative = (ISwapChainBackgroundPanelNative)(Object)scbp;

            return scbpNative;
        }

        public static implicit operator SwapChainBackgroundPanel(SwapChainBackgroundPanelInterop scbpInterop)
        {
            return scbpInterop.SCBP;
        }

        public static implicit operator SwapChainBackgroundPanelInterop(SwapChainBackgroundPanel scbp)
        {
            return new SwapChainBackgroundPanelInterop(scbp);
        }

    }
    
    public static class DXInteropHelper
    {
        private static bool _IsRunningOnBasicDisplayDriver;
        private static bool _IsRunningOnBasicDisplayDriverCheckDoneBefore = false;
        private static int _MaximumTextureSize = 0;

        public static void InitializeD3D(D3D_DRIVER_TYPE driverType, out ID3D11Device d3d11Device, D3D11_CREATE_DEVICE_FLAG flags = D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_BGRA_SUPPORT)
        {
            IntPtr d3d11DevicePtr;
            IntPtr d3d11ImmediateContextPtr;

#if DBG
            try
            {
                Assembly assembly = Assembly.Load("DXGIDebug.dll");
                flags |= D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_DEBUG;
            }
            catch (Exception e)
            {
            }
#endif

            D3D_FEATURE_LEVEL[] featureLevels = new D3D_FEATURE_LEVEL[]{
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_0,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_1,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_0,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_3,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_2,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_1
                };



            uint D3D11_SDK_VERSION = 7;


            D3D11CreateDevice(
                IntPtr.Zero,
                driverType,
                IntPtr.Zero,
                (uint)flags,
                featureLevels,
                (uint)featureLevels.Length,
                D3D11_SDK_VERSION,
                out d3d11DevicePtr,
                IntPtr.Zero,
                out d3d11ImmediateContextPtr);

            d3d11Device = (ID3D11Device)Marshal.GetObjectForIUnknown(d3d11DevicePtr);
            ((ID3D10Multithread)(object)d3d11Device).SetMultithreadProtected(1);

            Marshal.Release(d3d11ImmediateContextPtr);
        }

        public static void InitializeD2D(ID3D11Device d3d11Device, out DeviceContext d2dDeviceContext)
        {
            Factory1 d2dFactory = Factory1.Create(FactoryType.MultiThreaded);

            IntPtr dxgiDevicePtr = Marshal.GetComInterfaceForObject(d3d11Device, typeof(IDXGIDevice));
#if !TARGET_MOBILECORE
            Device d2dDevice = d2dFactory.CreateDevice(dxgiDevicePtr);
#else
            Device d2dDevice = d2dFactory.CreateDevice(dxgiDevicePtr.ToInt64());
#endif
            Marshal.Release(dxgiDevicePtr);

            d2dDeviceContext = d2dDevice.CreateDeviceContext(DeviceContextOptions.None);
        }

        public static bool IsRunningOnBasicDisplayDriver
        {
            get
            {
                return _IsRunningOnBasicDisplayDriverCheckDoneBefore ? _IsRunningOnBasicDisplayDriver : IsRunningOnBasicDisplayDriverImp();
            }
        }

        public static int GetMaximumTextureSize
        {
            get
            {
                return _MaximumTextureSize == 0 ? GetMaximumTextureSizeImp() : _MaximumTextureSize;
            }     
        }

        private static int GetMaximumTextureSizeImp()
        {
            IntPtr d3d11DevicePtr;
            IntPtr d3d11ImmediateContextPtr;
            ID3D11Device device;
            D3D_FEATURE_LEVEL featureLevel;
            _IsRunningOnBasicDisplayDriverCheckDoneBefore = true;

            D3D_FEATURE_LEVEL[] featureLevels = new D3D_FEATURE_LEVEL[]{
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_0,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_1,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_0,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_3,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_2,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_1
                };

            uint D3D11_SDK_VERSION = 7;

            D3D11CreateDevice(
                IntPtr.Zero,
                D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE,
                IntPtr.Zero,
                (uint)D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_BGRA_SUPPORT,
                featureLevels,
                (uint)featureLevels.Length,
                D3D11_SDK_VERSION,
                out d3d11DevicePtr,
                IntPtr.Zero,
                out d3d11ImmediateContextPtr);

            device = (ID3D11Device)Marshal.GetObjectForIUnknown(d3d11DevicePtr);

            featureLevel = device.GetFeatureLevel();
            switch (featureLevel)
            {
                case D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_1:
                case D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_2:
                    _MaximumTextureSize = 2048;
                    break;
                case D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_3:
                    _MaximumTextureSize = 4096;
                    break;
                case D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_0:
                case D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_1:
                    _MaximumTextureSize = 8192;
                    break;
                case D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_0:
                case D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_1:
                    _MaximumTextureSize = 16384;
                    break;
            }

            Marshal.Release(d3d11ImmediateContextPtr);
            Marshal.Release(d3d11DevicePtr);

            return _MaximumTextureSize;
        }

        private static Boolean IsRunningOnBasicDisplayDriverImp()
        {
            IntPtr d3d11DevicePtr;
            IntPtr d3d11ImmediateContextPtr;
            IDXGIDevice dxgiDevice;
            IDXGIAdapter dxgiAdapter;
            DXGI_ADAPTER_DESC desc = new DXGI_ADAPTER_DESC(){Description = new ushort[0x80]};

            string adapterType = null;
            _IsRunningOnBasicDisplayDriverCheckDoneBefore = true;

            D3D_FEATURE_LEVEL[] featureLevels = new D3D_FEATURE_LEVEL[]{
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_11_0,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_1,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_10_0,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_3,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_2,
                    D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_9_1
                };

            uint D3D11_SDK_VERSION = 7;

            D3D11CreateDevice(
                IntPtr.Zero,
                D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE,
                IntPtr.Zero,
                (uint)D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_BGRA_SUPPORT,
                featureLevels,
                (uint)featureLevels.Length,
                D3D11_SDK_VERSION,
                out d3d11DevicePtr,
                IntPtr.Zero,
                out d3d11ImmediateContextPtr);

            dxgiDevice = (IDXGIDevice)Marshal.GetObjectForIUnknown(d3d11DevicePtr);
            dxgiDevice.GetAdapter(out dxgiAdapter);
            try
            {
                dxgiAdapter.GetDesc(ref desc);
            }
            catch
            {
            }

            unsafe
            {
                fixed (ushort* firstCharAddress = &desc.Description[0])
                    adapterType = new string((char*)firstCharAddress, 0, desc.Description.Length);
            }

            Marshal.Release(d3d11ImmediateContextPtr);
            Marshal.Release(d3d11DevicePtr);

            if (adapterType.Contains("Microsoft Basic Render Driver"))
            {
                _IsRunningOnBasicDisplayDriver = true;
                return true;
            }
            else
            {
                _IsRunningOnBasicDisplayDriver = false;
                return false;
            }
        }

        public static IDXGIFactory2 CreateDXGIFactory()
        {
            IDXGIFactory2 dxgiFactory = null;
            IntPtr dxgiFactoryPtr = IntPtr.Zero;

            Guid dxgiFactoryGuid = new Guid("50c83a1c-e072-4c48-87b0-3630fa36a6d0");
            DXInteropHelper.CreateDXGIFactoryNative(ref dxgiFactoryGuid, out dxgiFactoryPtr);
            dxgiFactory = (IDXGIFactory2)Marshal.GetObjectForIUnknown(dxgiFactoryPtr);
            Marshal.Release(dxgiFactoryPtr);

            return dxgiFactory;
        }

        public static IDXGISwapChain1 CreateSwapChainForComposition(ID3D11Device device, int width, int height, bool enableDFlip = false)
        {
            IDXGISwapChain1 swapchain = null;

#if !TARGET_MOBILECORE //PS 57492 - Windows.UI.ViewManagement.ApplicationViewState is not available on the phone
            enableDFlip = CheckDFlipSupported(enableDFlip);
#endif

            if (enableDFlip)
            {
                //always create 2 X 1 swapchain to support Dflip
                if (width < height)
                {
                    int temp = width;
                    width = height;
                    height = temp;
                }
            }

            DXGI_SWAP_CHAIN_DESC1 swapChainDesc = new DXGI_SWAP_CHAIN_DESC1();
            swapChainDesc.Width = (uint)width;
            swapChainDesc.Height = (uint)height;
            swapChainDesc.Format = ManagedJupiterHost.DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM;
            swapChainDesc.Stereo = 0;
            swapChainDesc.SampleDesc.Count = 1;
            swapChainDesc.SampleDesc.Quality = 0;
            swapChainDesc.BufferUsage = (uint)DXGI_USAGE.DXGI_USAGE_RENDER_TARGET_OUTPUT;
            swapChainDesc.BufferCount = 2;
            swapChainDesc.Scaling = DXGI_SCALING.DXGI_SCALING_STRETCH;
            swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT.DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL;
            swapChainDesc.Flags = 0;
            swapChainDesc.AlphaMode = DXGI_ALPHA_MODE.DXGI_ALPHA_MODE_UNSPECIFIED; // Don't use DXGI_ALPHA_MODE_PREMULTIPLIED as that will affect perf by forcing a composition of an opaque surface.

            IDXGIFactory2 dxgiFactory = DXInteropHelper.CreateDXGIFactory();
            dxgiFactory.CreateSwapChainForComposition(device, ref swapChainDesc, null, out swapchain);

            if (enableDFlip)
            {
                switch (DisplayProperties.CurrentOrientation)
                {
                    case DisplayOrientations.Landscape:
                        swapchain.SetRotation(DXGI_MODE_ROTATION.DXGI_MODE_ROTATION_IDENTITY);
                        break;
                    case DisplayOrientations.Portrait:
                        swapchain.SetRotation(DXGI_MODE_ROTATION.DXGI_MODE_ROTATION_ROTATE270);
                        break;
                    case DisplayOrientations.LandscapeFlipped:
                        swapchain.SetRotation(DXGI_MODE_ROTATION.DXGI_MODE_ROTATION_ROTATE180);
                        break;
                    case DisplayOrientations.PortraitFlipped:
                        swapchain.SetRotation(DXGI_MODE_ROTATION.DXGI_MODE_ROTATION_ROTATE90);
                        break;
                }
            }

            return swapchain;
        }

        private static bool CheckDFlipSupported(bool enableDFlip)
        {
            if (Windows.UI.ViewManagement.ApplicationView.Value != Windows.UI.ViewManagement.ApplicationViewState.FullScreenLandscape
                && Windows.UI.ViewManagement.ApplicationView.Value != Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
            {
                //override user request since dflip makes sense only with full screen modes.
                enableDFlip = false;
            }
            return enableDFlip;
        }



        public static void Render(IDXGISurface scratchSurface, IDXGIDevice dxgiDevice, float[] color)
        {

            ID3D11Device d3d11Device = DXInteropHelper.GetD3D11Device(dxgiDevice);
            ID3D11DeviceContext d3d11ImmediateContext = null;
            d3d11Device.GetImmediateContext(ref d3d11ImmediateContext);
            ID3D11Resource d3d11Resource = (ID3D11Resource)(Object)scratchSurface;

            ID3D11RenderTargetView d3d11RenderTargetView = null;
            D3D11_RENDER_TARGET_VIEW_DESC desc = new D3D11_RENDER_TARGET_VIEW_DESC();
            desc.Format = ManagedJupiterHost.DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM;
            desc.ViewDimension = D3D11_RTV_DIMENSION.D3D11_RTV_DIMENSION_TEXTURE2D;
            desc.__MIDL____MIDL_itf_d3d11_0001_00910002.Texture2D.MipSlice = 0;

            d3d11Device.CreateRenderTargetView(d3d11Resource, ref desc, ref d3d11RenderTargetView);

            d3d11ImmediateContext.ClearRenderTargetView(d3d11RenderTargetView, color);

            d3d11ImmediateContext.ClearState();
            Marshal.ReleaseComObject(d3d11ImmediateContext);
            Marshal.ReleaseComObject(d3d11RenderTargetView);
            Marshal.ReleaseComObject(d3d11Resource);
        }

        public static bool HasUIThreadAccess()
        {
            try
            {
                return Windows.UI.Xaml.Window.Current.Dispatcher.HasThreadAccess;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static void RenderWithD2DImpl(Bitmap1 bitmap, DeviceContext d2dDeviceContext, Action<double, double> renderFunc)
        {
            d2dDeviceContext.SetTarget(bitmap);

            d2dDeviceContext.BeginDraw();

#if !TARGET_MOBILECORE
            int surfaceWidth = (int)bitmap.GetSize().Width;
            int surfaceHeight = (int)bitmap.GetSize().Height;
#else
            int surfaceWidth=100, surfaceHeight=100; //need to fix this in mobilecore.
#endif

            renderFunc(surfaceWidth, surfaceHeight);
            d2dDeviceContext.SetTarget(null);

#if !TARGET_MOBILECORE
            Tag? tag = null;
            d2dDeviceContext.EndDraw(ref tag, ref tag);
#else
            d2dDeviceContext.EndDraw(null, null);
#endif
            bitmap.Dispose();
        }

        public static Bitmap1 GetD2DBitmapFromSurface(IDXGISurface surface, DeviceContext d2dDeviceContext)
        {
            IntPtr scratchSurfacePtr = Marshal.GetComInterfaceForObject(surface, typeof(IDXGISurface));

#if !TARGET_MOBILECORE
            BitmapProperties1? bitmapProperties = null;
            Bitmap1 bitmap = d2dDeviceContext.CreateBitmapFromDxgiSurface(scratchSurfacePtr, ref bitmapProperties);
#else
            Bitmap1 bitmap = d2dDeviceContext.CreateBitmapFromDxgiSurface(scratchSurfacePtr.ToInt64(), null);
#endif
            Marshal.Release(scratchSurfacePtr);
            return bitmap;
        }

        public static void RenderWithD2D(Bitmap1 bitmap, DeviceContext d2dDeviceContext, float[] color)
        {
            RenderWithD2DImpl(bitmap, d2dDeviceContext, (double surfaceWidth, double surfaceHeight) =>
            {
#if !TARGET_MOBILECORE
                ColorF? fillColor = new ColorF(color[0], color[1], color[2], color[3]);
                d2dDeviceContext.Clear(ref fillColor);
#endif
            });
        }

        public static void RenderTilesWithD2D(Bitmap1 bitmap, DeviceContext d2dDeviceContext)
        {
            RenderWithD2DImpl(bitmap, d2dDeviceContext, (double surfaceWidth, double surfaceHeight) =>
            {
#if !TARGET_MOBILECORE
                int tileSize = 100;

                ColorF color = new ColorF(1, 0, 0, 1);
                BrushProperties? brushProperties = null;
                SolidColorBrush brush1 = d2dDeviceContext.CreateSolidColorBrush(ref color, ref brushProperties);
                color = new ColorF(0, 1, 0, 1);
                SolidColorBrush brush2 = d2dDeviceContext.CreateSolidColorBrush(ref color, ref brushProperties);
                color = new ColorF(0, 0, 1, 1);
                SolidColorBrush brush3 = d2dDeviceContext.CreateSolidColorBrush(ref color, ref brushProperties);

                Brush[] brushes = { brush1, brush2, brush3 };

                int i = 0;
                for (int left = 0; left < surfaceWidth; left += tileSize, i++)
                {
                    for (int top = 0, j = i; top < surfaceHeight; top += tileSize, j++)
                    {
                        RectF rect;
                        rect.left = (int)Math.Min(left, surfaceWidth);
                        rect.top = (int)Math.Min(top, surfaceHeight);
                        rect.right = left + (int)Math.Min(left + tileSize, surfaceWidth);
                        rect.bottom = top + (int)Math.Min(top + tileSize, surfaceHeight);

                        d2dDeviceContext.FillRectangle(ref rect, brushes[j % brushes.Length]);
                    }
                }
#endif
            });
        }

        public static void RenderClockWithD2D(Bitmap1 bitmap, DeviceContext d2dDeviceContext, float[] color, float width, float height, bool enableDFlip = false)
        {
            RenderWithD2DImpl(bitmap, d2dDeviceContext, (double surfaceWidth, double surfaceHeight) =>
            {
                enableDFlip = DXInteropHelper.CheckDFlipSupported(enableDFlip);

                RenderClock(new ColorF(color[0], color[1], color[2], color[3]), d2dDeviceContext, width, height, enableDFlip);
            });
        }

        public static void RenderEllipseWithD2D(Bitmap1 bitmap, DeviceContext d2dDeviceContext, Point centerPoint, float radius, float[] fillColor)
        {
            RenderWithD2DImpl(bitmap, d2dDeviceContext, (double surfaceWidth, double surfaceHeight) =>
            {
                ColorF color = new ColorF(fillColor[0], fillColor[1], fillColor[2], fillColor[3]);
                Ellipse ellipse = new Ellipse(new Point2F((float)centerPoint.X, (float)centerPoint.Y), radius, radius);

#if !TARGET_MOBILECORE
                BrushProperties? brushProperties = null;
                SolidColorBrush fill = d2dDeviceContext.CreateSolidColorBrush(ref color, ref brushProperties);
                d2dDeviceContext.FillEllipse(ref ellipse, fill);
#else
                SolidColorBrush fill = d2dDeviceContext.CreateSolidColorBrush(color, null);
                d2dDeviceContext.FillEllipse(ellipse, fill);
#endif
            });
        }

        public static void RenderRectangleWithD2D(Bitmap1 bitmap, DeviceContext d2dDeviceContext, float[] color, Rect rect)
        {
            RenderWithD2DImpl(bitmap, d2dDeviceContext, (double surfaceWidth, double surfaceHeight) =>
            {
#if !TARGET_MOBILECORE
                RectF d2dRect = new RectF((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom);
                ColorF fillColor = new ColorF(color[0], color[1], color[2], color[3]);
                BrushProperties? brushProperties = null;
                SolidColorBrush fill = d2dDeviceContext.CreateSolidColorBrush(ref fillColor, ref brushProperties);
                d2dDeviceContext.FillRectangle(ref d2dRect, fill);
#endif
            });
        }

        public static Device CreateD2DDevice(ID3D11Device d3d11Device, FactoryType factoryType = FactoryType.MultiThreaded)
        {
            Factory1 d2dFactory = Factory1.Create(factoryType);
            IntPtr dxgiDevicePtr = Marshal.GetComInterfaceForObject(d3d11Device, typeof(IDXGIDevice));
#if !TARGET_MOBILECORE
            Device d2dDevice = d2dFactory.CreateDevice(dxgiDevicePtr);
#else
            Device d2dDevice = d2dFactory.CreateDevice(dxgiDevicePtr.ToInt64());
#endif
            Marshal.Release(dxgiDevicePtr);
            return d2dDevice;
        }

        private static void RenderClock(ColorF clockColor, DeviceContext d2dDeviceContext, float width, float height, bool enableDFlip = false)
        {
            //Porting code from http://msdn.microsoft.com/en-us/library/windows/desktop/ff819063(v=vs.85).aspx

            Matrix3x2F DFlipRotationTranformMatrix = new Matrix3x2F(1, 0, 0, 1, 0, 0); // identity.

            if (enableDFlip)
            {
                if (width < height)
                {
                    float temp = width;
                    width = height;
                    height = temp;
                }

                switch (DisplayProperties.CurrentOrientation)
                {
                    case DisplayOrientations.Landscape:
                        DFlipRotationTranformMatrix = new Matrix3x2F(1, 0, 0, 1, 0, 0); // identity.
                        break;
                    case DisplayOrientations.Portrait:
                        DFlipRotationTranformMatrix = new Matrix3x2F().Rotation(270, new Point2F(width / 2, height / 2));
                        break;
                    case DisplayOrientations.LandscapeFlipped:
                        DFlipRotationTranformMatrix = new Matrix3x2F().Rotation(180, new Point2F(width / 2, height / 2));
                        break;
                    case DisplayOrientations.PortraitFlipped:
                        DFlipRotationTranformMatrix = new Matrix3x2F().Rotation(90, new Point2F(width / 2, height / 2));
                        break;
                }
            }

            float x = (float)(width / 2.0);
            float y = (float)(height / 2.0);
            float radius = Math.Min(x, y);
            Ellipse ellipse = new Ellipse(new Point2F(x, y), radius, radius);


            // Calculate tick marks.
            Point2F[] ticks = new Point2F[24];

            Point2F point1 = new Point2F(ellipse.Point.X, ellipse.Point.Y - (ellipse.RadiusY * 0.9f));
            Point2F point2 = new Point2F(ellipse.Point.X, ellipse.Point.Y - ellipse.RadiusY);

            for (int i = 0; i < 12; i++)
            {
                Matrix3x2F matrix = new Matrix3x2F().Rotation((360 / 12) * i, ellipse.Point);

                ticks[i * 2] = matrix.TransformPoint(point1);
                ticks[i * 2 + 1] = matrix.TransformPoint(point2);
            }

            ColorF color = new ColorF(0, 0, 0, 1); //Black.
#if !TARGET_MOBILECORE
            BrushProperties? brushProperties = null;
            SolidColorBrush stroke = d2dDeviceContext.CreateSolidColorBrush(ref color, ref brushProperties);
            ColorF? nullableColor = new ColorF(0, 0, 1, 1); //Blue
            d2dDeviceContext.Clear(ref nullableColor);
            SolidColorBrush fill = d2dDeviceContext.CreateSolidColorBrush(ref clockColor, ref brushProperties);
            d2dDeviceContext.FillEllipse(ref ellipse, fill);
            d2dDeviceContext.DrawEllipse(ref ellipse, stroke, 2.0f, null);
#else
            SolidColorBrush stroke = d2dDeviceContext.CreateSolidColorBrush(color, null);
            ColorF nullableColor = new ColorF(0, 0, 1, 1); //Blue
            d2dDeviceContext.Clear(nullableColor);
            SolidColorBrush fill = d2dDeviceContext.CreateSolidColorBrush(clockColor, null);
            d2dDeviceContext.FillEllipse(ellipse, fill);
            d2dDeviceContext.DrawEllipse(ellipse, stroke, 2.0f, null);
#endif

            // Draw tick marks
            for (int i = 0; i < 12; i++)
            {
                d2dDeviceContext.DrawLine(ticks[i * 2], ticks[i * 2 + 1], stroke, 2.0f, null);
            }

            DateTime time = System.DateTime.Now;

            // 60 minutes = 30 degrees, 1 minute = 0.5 degree
            float fHourAngle = (360.0f / 12) * (time.Hour) + (time.Minute * 0.5f);
            float fMinuteAngle = (360.0f / 60) * (time.Minute);
            float fSecondAngle =
              (360.0f / 60) * (time.Second) + (360.0f / 60000) * (time.Millisecond);

            DrawClockHand(0.6f, fHourAngle, 6, d2dDeviceContext, ellipse, stroke, DFlipRotationTranformMatrix);
            DrawClockHand(0.85f, fMinuteAngle, 4, d2dDeviceContext, ellipse, stroke, DFlipRotationTranformMatrix);
            DrawClockHand(0.85f, fSecondAngle, 1, d2dDeviceContext, ellipse, stroke, DFlipRotationTranformMatrix);

            // Restore the identity transformation.
            Matrix3x2F matrix1 = new Matrix3x2F(1, 0, 0, 1, 0, 0); // identity.
#if !TARGET_MOBILECORE
            d2dDeviceContext.SetTransform(ref matrix1);
#else
            d2dDeviceContext.SetTransform(matrix1);
#endif
        }

        private static void DrawClockHand(float fHandLength, float fAngle, float fStrokeWidth, DeviceContext d2dDeviceContext, Ellipse ellipse, SolidColorBrush stroke, Matrix3x2F DFlipRotationTranformMatrix)
        {
            Matrix3x2F rotationMatrix = new Matrix3x2F().Rotation(fAngle, ellipse.Point).Multiply(DFlipRotationTranformMatrix);
#if !TARGET_MOBILECORE
            d2dDeviceContext.SetTransform(ref rotationMatrix);
#else
            d2dDeviceContext.SetTransform(rotationMatrix);
#endif

            // endPoint defines one end of the hand.
            Point2F endPoint = new Point2F(
                ellipse.Point.X,
                ellipse.Point.Y - (ellipse.RadiusY * fHandLength)
                );

            // Draw a line from the center of the ellipse to endPoint.
            d2dDeviceContext.DrawLine(ellipse.Point, endPoint, stroke, fStrokeWidth, null);
        }

        [System.Runtime.InteropServices.DllImportAttribute("d3d11.dll", EntryPoint = "D3D11CreateDevice", CallingConvention = CallingConvention.StdCall)]
        public static extern int D3D11CreateDevice(
            IntPtr pAdapter,
            D3D_DRIVER_TYPE DriverType,
            System.IntPtr Software,
            uint Flags,
            [MarshalAs(UnmanagedType.LPArray)]  D3D_FEATURE_LEVEL[] pFeatureLevels,
            uint FeatureLevels,
            uint SDKVersion,
            out System.IntPtr ppDevice,
            IntPtr pFeatureLevel,
            out IntPtr ppImmediateContext);

        [System.Runtime.InteropServices.DllImportAttribute("dxgi.dll", EntryPoint = "CreateDXGIFactory", CallingConvention = CallingConvention.StdCall)]
        private static extern int CreateDXGIFactoryNative(ref Guid riid, out  IntPtr ppFactory);


        public static IDXGIDevice GetDXGIDevice(ID3D11Device d3d11Device)
        {
            IDXGIDevice dxgiDevice = (IDXGIDevice)(Object)d3d11Device;

            return dxgiDevice;

        }

        public static ID3D11Device GetD3D11Device(IDXGIDevice dxgiDevice)
        {
            ID3D11Device d3d11Device = (ID3D11Device)(Object)dxgiDevice;
            return d3d11Device;
        }
    }

    public static class Extensions
    {
        public static Matrix3x2F Multiply(this Matrix3x2F a, Matrix3x2F b)
        {
            Matrix3x2F matrix = new Matrix3x2F();
            matrix._11 = a._11 * b._11 + a._12 * b._21;
            matrix._12 = a._11 * b._12 + a._12 * b._22;
            matrix._21 = a._21 * b._11 + a._22 * b._21;
            matrix._22 = a._21 * b._12 + a._22 * b._22;
            matrix._31 = a._31 * b._11 + a._32 * b._21 + b._31;
            matrix._32 = a._31 * b._12 + a._32 * b._22 + b._32;

            return matrix;
        }

        public static Point2F TransformPoint(this Matrix3x2F matrix, Point2F point)
        {
            Point2F pt = new Point2F();
            pt.X = point.X * matrix._11 + point.Y * matrix._21 + matrix._31;
            pt.Y = point.X * matrix._12 + point.Y * matrix._22 + matrix._32;
            return pt;
        }


        public static Matrix3x2F Rotation(this Matrix3x2F matrix, double degrees, Point2F center)
        {
            float sinAngle = (float)Math.Sin(degrees * Math.PI / 180);
            float cosAngle = (float)Math.Cos(degrees * Math.PI / 180);

            matrix._11 = matrix._22 = cosAngle;
            matrix._12 = sinAngle;
            matrix._21 = -sinAngle;
            matrix._31 = (center.X * (1.0f - cosAngle)) + (center.Y * sinAngle);
            matrix._32 = (center.Y * (1.0f - cosAngle)) - (center.X * sinAngle);

            return matrix;
        }

        public static Matrix3x2F Translation(this Matrix3x2F matrix, SizeF size)
        {
            matrix._11 = (float)1.0; matrix._12 = (float)0.0;
            matrix._21 = (float)0.0; matrix._22 = (float)1.0;
            matrix._31 = size.Width; matrix._32 = size.Height;

            return matrix;
        }

    }
}