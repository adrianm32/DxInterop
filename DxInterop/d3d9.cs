using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Direct3D9
{
    [ComImport, Guid("02177241-69FC-400C-8FF1-93A44DF6861D"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDirect3D9Ex
    {
        int RegisterSoftwareDevice();
        int GetAdapterCount();
        int GetAdapterIdentifier();
        int GetAdapterModeCount();
        int EnumAdapterModes();
        int GetAdapterDisplayMode();
        int CheckDeviceType();
        int CheckDeviceFormat();
        int CheckDeviceMultiSampleType();
        int CheckDepthStencilMatch();
        int CheckDeviceFormatConversion();
        int GetDeviceCaps();
        int GetAdapterMonitor();
        int CreateDevice();
        int GetAdapterModeCountEx();
        int EnumAdapterModesEx();
        int GetAdapterDisplayModeEx();
        void CreateDeviceEx(
            D3DADAPTER Adapter,
            D3DDEVTYPE DeviceType,
            IntPtr hFocusWindow,
            D3DCREATE BehaviorFlags,
            [In, Out] ref D3DPRESENT_PARAMETERS pPresentationParameters,
            [In, Out] IntPtr pFullscreenDisplayMode,
            [Out] out IDirect3DDevice9Ex ppReturnedDeviceInterface);
        int GetAdapterLUID();
    }

    [ComImport, Guid("B18B10CE-2649-405a-870F-95F777D4313A"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDirect3DDevice9Ex
    {
        int TestCooperativeLevel();
        int GetAvailableTextureMem();
        int EvictManagedResources();
        int GetDirect3D();
        int GetDeviceCaps();
        int GetDisplayMode();
        int GetCreationParameters();
        int SetCursorProperties();
        int SetCursorPosition();
        int ShowCursor();
        int CreateAdditionalSwapChain();
        int GetSwapChain();
        int GetNumberOfSwapChains();
        int Reset();
        int Present();
        int GetBackBuffer();
        int GetRasterStatus();
        int SetDialogBoxMode();
        int SetGammaRamp();
        int GetGammaRamp();
        void CreateTexture(uint width, uint height, uint levels, D3DUSAGE usage, D3DFORMAT format, D3DPOOL pool, [Out] out IDirect3DTexture9 texture, ref IntPtr sharedHandle);
        int CreateVolumeTexture();
        int CreateCubeTexture();
        int CreateVertexBuffer();
        int CreateIndexBuffer();
        int CreateRenderTarget();
        int CreateDepthStencilSurface();
        int UpdateSurface();
        int UpdateTexture();
        int GetRenderTargetData();
        int GetFrontBufferData();
        void StretchRect(IntPtr pSourceSurface, IntPtr pSourceRect, IntPtr pDestSurface, IntPtr pDestRect, D3DTEXTUREFILTERTYPE Filter);
        int ColorFill();
        int CreateOffscreenPlainSurface();
        int SetRenderTarget();
        int GetRenderTarget();
        int SetDepthStencilSurface();
        int GetDepthStencilSurface();
        int BeginScene();
        int EndScene();
        int Clear();
        int SetTransform();
        int GetTransform();
        int MultiplyTransform();
        int SetViewport();
        int GetViewport();
        int SetMaterial();
        int GetMaterial();
        int SetLight();
        int GetLight();
        int LightEnable();
        int GetLightEnable();
        int SetClipPlane();
        int GetClipPlane();
        int SetRenderState();
        int GetRenderState();
        int CreateStateBlock();
        int BeginStateBlock();
        int EndStateBlock();
        int SetClipStatus();
        int GetClipStatus();
        int GetTexture();
        int SetTexture();
        int GetTextureStageState();
        int SetTextureStageState();
        int GetSamplerState();
        int SetSamplerState();
        int ValidateDevice();
        int SetPaletteEntries();
        int GetPaletteEntries();
        int SetCurrentTexturePalette();
        int GetCurrentTexturePalette();
        int SetScissorRect();
        int GetScissorRect();
        int SetSoftwareVertexProcessing();
        int GetSoftwareVertexProcessing();
        int SetNPatchMode();
        int GetNPatchMode();
        int DrawPrimitive();
        int DrawIndexedPrimitive();
        int DrawPrimitiveUP();
        int DrawIndexedPrimitiveUP();
        int ProcessVertices();
        int CreateVertexDeclaration();
        int SetVertexDeclaration();
        int GetVertexDeclaration();
        int SetFVF();
        int GetFVF();
        int CreateVertexShader();
        int SetVertexShader();
        int GetVertexShader();
        int SetVertexShaderConstantF();
        int GetVertexShaderConstantF();
        int SetVertexShaderConstantI();
        int GetVertexShaderConstantI();
        int SetVertexShaderConstantB();
        int GetVertexShaderConstantB();
        int SetStreamSource();
        int GetStreamSource();
        int SetStreamSourceFreq();
        int GetStreamSourceFreq();
        int SetIndices();
        int GetIndices();
        int CreatePixelShader();
        int SetPixelShader();
        int GetPixelShader();
        int SetPixelShaderConstantF();
        int GetPixelShaderConstantF();
        int SetPixelShaderConstantI();
        int GetPixelShaderConstantI();
        int SetPixelShaderConstantB();
        int GetPixelShaderConstantB();
        int DrawRectPatch();
        int DrawTriPatch();
        int DeletePatch();
        int CreateQuery();
        int SetConvolutionMonoKernel();
        int ComposeRects();
        int PresentEx();
        int GetGPUThreadPriority();
        int SetGPUThreadPriority();
        int WaitForVBlank();
        int CheckResourceResidency();
        int SetMaximumFrameLatency();
        int GetMaximumFrameLatency();
        int CheckDeviceState();
        int CreateRenderTargetEx();
        int CreateOffscreenPlainSurfaceEx();
        int CreateDepthStencilSurfaceEx();
        int ResetEx();
        int GetDisplayModeEx();
    }

    [ComImport, Guid("85C31227-3DE5-4f00-9B3A-F11AC38C18B5"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDirect3DTexture9
    {
        int GetDevice();
        int SetPrivateData();
        int GetPrivateData();
        int FreePrivateData();
        int SetPriority();
        int GetPriority();
        int PreLoad();
        int GetType();
        int SetLOD();
        int GetLOD();
        int GetLevelCount();
        int SetAutoGenFilterType();
        int GetAutoGenFilterType();
        int GenerateMipSubLevels();
        int GetLevelDesc();
        void GetSurfaceLevel(uint level, [Out] out IntPtr surfaceLevel);
        int LockRect();
        int UnlockRect();
        int AddDirtyRect();
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct D3DPRESENT_PARAMETERS
    {
        public uint BackBufferWidth;
        public uint BackBufferHeight;
        public D3DFORMAT BackBufferFormat;
        public uint BackBufferCount;
        public D3DMULTISAMPLE_TYPE MultiSampleType;
        public int MultiSampleQuality;
        public D3DSWAPEFFECT SwapEffect;
        public IntPtr hDeviceWindow;
        public int Windowed;
        public int EnableAutoDepthStencil;
        public D3DFORMAT AutoDepthStencilFormat;
        public int Flags;
        /* FullScreen_RefreshRateInHz must be zero for Windowed mode */
        public uint FullScreen_RefreshRateInHz;
        public D3DPRESENT_INTERVAL PresentationInterval;
    }

    [Flags]
    public enum D3DPOOL
    {
        D3DPOOL_DEFAULT = 0,
        D3DPOOL_MANAGED = 1,
        D3DPOOL_SYSTEMMEM = 2,
        D3DPOOL_SCRATCH = 3,
        D3DPOOL_FORCE_DWORD = 0x7fffffff
    }

    [Flags]
    public enum D3DUSAGE
    {
        D3DUSAGE_RENDERTARGET = 0x00000001,
        D3DUSAGE_DEPTHSTENCIL = 0x00000002,
        D3DUSAGE_DYNAMIC = 0x00000200
    }

    [Flags]
    public enum D3DRESOURCETYPE
    {
        D3DRTYPE_SURFACE = 1,
        D3DRTYPE_VOLUME = 2,
        D3DRTYPE_TEXTURE = 3,
        D3DRTYPE_VOLUMETEXTURE = 4,
        D3DRTYPE_CUBETEXTURE = 5,
        D3DRTYPE_VERTEXBUFFER = 6,
        D3DRTYPE_INDEXBUFFER = 7,           //if this changes, change _D3DDEVINFO_RESOURCEMANAGER definition


        D3DRTYPE_FORCE_DWORD = 0x7fffffff
    }

    [Flags]
    public enum D3DADAPTER
    {
        D3DADAPTER_DEFAULT = 0
    }

    [Flags]
    public enum D3DCREATE
    {
        D3DCREATE_FPU_PRESERVE = 0x00000002,
        D3DCREATE_MULTITHREADED = 0x00000004,
        D3DCREATE_PUREDEVICE = 0x00000010,
        D3DCREATE_SOFTWARE_VERTEXPROCESSING = 0x00000020,
        D3DCREATE_HARDWARE_VERTEXPROCESSING = 0x00000040,
        D3DCREATE_MIXED_VERTEXPROCESSING = 0x00000080,
        D3DCREATE_DISABLE_DRIVER_MANAGEMENT = 0x00000100,
        D3DCREATE_ADAPTERGROUP_DEVICE = 0x00000200,
        D3DCREATE_DISABLE_DRIVER_MANAGEMENT_EX = 0x00000400
    }

    public enum D3DDEVTYPE
    {
        D3DDEVTYPE_HAL = 1,
        D3DDEVTYPE_REF = 2,
        D3DDEVTYPE_SW = 3,
        D3DDEVTYPE_NULLREF = 4,
    }

    public enum D3DFORMAT
    {
        D3DFMT_UNKNOWN = 0,
        D3DFMT_R8G8B8 = 20,
        D3DFMT_A8R8G8B8 = 21,
        D3DFMT_X8R8G8B8 = 22,
        D3DFMT_R5G6B5 = 23,
        D3DFMT_X1R5G5B5 = 24,
        D3DFMT_A1R5G5B5 = 25,
        D3DFMT_A4R4G4B4 = 26,
        D3DFMT_R3G3B2 = 27,
        D3DFMT_A8 = 28,
        D3DFMT_A8R3G3B2 = 29,
        D3DFMT_X4R4G4B4 = 30,
        D3DFMT_A2B10G10R10 = 31,
        D3DFMT_A8B8G8R8 = 32,
        D3DFMT_X8B8G8R8 = 33,
        D3DFMT_G16R16 = 34,
        D3DFMT_A2R10G10B10 = 35,
        D3DFMT_A16B16G16R16 = 36,
        D3DFMT_A8P8 = 40,
        D3DFMT_P8 = 41,
        D3DFMT_L8 = 50,
        D3DFMT_A8L8 = 51,
        D3DFMT_A4L4 = 52,
        D3DFMT_V8U8 = 60,
        D3DFMT_L6V5U5 = 61,
        D3DFMT_X8L8V8U8 = 62,
        D3DFMT_Q8W8V8U8 = 63,
        D3DFMT_V16U16 = 64,
        D3DFMT_A2W10V10U10 = 67,
        D3DFMT_D16_LOCKABLE = 70,
        D3DFMT_D32 = 71,
        D3DFMT_D15S1 = 73,
        D3DFMT_D24S8 = 75,
        D3DFMT_D24X8 = 77,
        D3DFMT_D24X4S4 = 79,
        D3DFMT_D16 = 80,
        D3DFMT_D32F_LOCKABLE = 82,
        D3DFMT_D24FS8 = 83,
        /* Z-Stencil formats valid for CPU access */
        D3DFMT_D32_LOCKABLE = 84,
        D3DFMT_S8_LOCKABLE = 85,
        D3DFMT_L16 = 81,
        D3DFMT_VERTEXDATA = 100,
        D3DFMT_INDEX16 = 101,
        D3DFMT_INDEX32 = 102,
        D3DFMT_Q16W16V16U16 = 110,
        // Floating point surface formats
        // s10e5 formats (16-bits per channel)
        D3DFMT_R16F = 111,
        D3DFMT_G16R16F = 112,
        D3DFMT_A16B16G16R16F = 113,
        // IEEE s23e8 formats (32-bits per channel)
        D3DFMT_R32F = 114,
        D3DFMT_G32R32F = 115,
        D3DFMT_A32B32G32R32F = 116,
        D3DFMT_CxV8U8 = 117,
        // Monochrome 1 bit per pixel format
        D3DFMT_A1 = 118,
        // Binary format indicating that the data has no inherent type
        D3DFMT_BINARYBUFFER = 199,
    }

    public enum D3DSWAPEFFECT
    {
        D3DSWAPEFFECT_DISCARD = 1,
        D3DSWAPEFFECT_FLIP = 2,
        D3DSWAPEFFECT_COPY = 3,
    }

    public enum D3DMULTISAMPLE_TYPE
    {
        D3DMULTISAMPLE_NONE = 0,
        D3DMULTISAMPLE_NONMASKABLE = 1,
        D3DMULTISAMPLE_2_SAMPLES = 2,
        D3DMULTISAMPLE_3_SAMPLES = 3,
        D3DMULTISAMPLE_4_SAMPLES = 4,
        D3DMULTISAMPLE_5_SAMPLES = 5,
        D3DMULTISAMPLE_6_SAMPLES = 6,
        D3DMULTISAMPLE_7_SAMPLES = 7,
        D3DMULTISAMPLE_8_SAMPLES = 8,
        D3DMULTISAMPLE_9_SAMPLES = 9,
        D3DMULTISAMPLE_10_SAMPLES = 10,
        D3DMULTISAMPLE_11_SAMPLES = 11,
        D3DMULTISAMPLE_12_SAMPLES = 12,
        D3DMULTISAMPLE_13_SAMPLES = 13,
        D3DMULTISAMPLE_14_SAMPLES = 14,
        D3DMULTISAMPLE_15_SAMPLES = 15,
        D3DMULTISAMPLE_16_SAMPLES = 16,
    }

    [Flags]
    public enum D3DPRESENTFLAG
    {
        D3DPRESENTFLAG_LOCKABLE_BACKBUFFER = 0x00000001,
        D3DPRESENTFLAG_DISCARD_DEPTHSTENCIL = 0x00000002,
        D3DPRESENTFLAG_DEVICECLIP = 0x00000004,
        D3DPRESENTFLAG_VIDEO = 0x00000010
    }

    [Flags]
    public enum D3DPRESENT_INTERVAL : uint
    {
        D3DPRESENT_INTERVAL_DEFAULT = 0x00000000,
        D3DPRESENT_INTERVAL_ONE = 0x00000001,
        D3DPRESENT_INTERVAL_TWO = 0x00000002,
        D3DPRESENT_INTERVAL_THREE = 0x00000004,
        D3DPRESENT_INTERVAL_FOUR = 0x00000008,
        D3DPRESENT_INTERVAL_IMMEDIATE = 0x80000000
    }

    public enum D3DTEXTUREFILTERTYPE : uint
    {
        D3DTEXF_NONE = 0,
        D3DTEXF_POINT = 1,
        D3DTEXF_LINEAR = 2,
        D3DTEXF_ANISOTROPIC = 3,
        D3DTEXF_PYRAMIDALQUAD = 6,
        D3DTEXF_GAUSSIANQUAD = 7,
        D3DTEXF_CONVOLUTIONMONO = 8,
        D3DTEXF_FORCE_DWORD = 0x7fffffff
    }

    public class Direct3D
    {
        public const int D3D_SDK_VERSION = 32;

        [DllImport("d3d9.dll", EntryPoint = "Direct3DCreate9Ex", PreserveSig = false)]
        public static extern void Direct3DCreate9Ex([In] uint SDKVersion, [Out] out IDirect3D9Ex d3d);
    }
}
