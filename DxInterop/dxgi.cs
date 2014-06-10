namespace ManagedJupiterHost
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit, Size = 4, Pack = 4)]
    public struct __MIDL_IWinTypes_0009
    {
        [FieldOffset(0)]
        public int hInproc;
        [FieldOffset(0)]
        public int hRemote;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct _LARGE_INTEGER
    {
        public long QuadPart;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct _LUID
    {
        public uint LowPart;
        public int HighPart;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct _RemotableHandle
    {
        public int fContext;
        public __MIDL_IWinTypes_0009 u;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_ADAPTER_DESC
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x80)]
        public ushort[] Description;
        public uint VendorId;
        public uint DeviceId;
        public uint SubSysId;
        public uint Revision;

        public uint DedicatedVideoMemory;

        public uint DedicatedSystemMemory;

        public uint SharedSystemMemory;
        public _LUID AdapterLuid;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_ADAPTER_DESC1
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x80)]
        public ushort[] Description;
        public uint VendorId;
        public uint DeviceId;
        public uint SubSysId;
        public uint Revision;
        public uint DedicatedVideoMemory;
        public uint DedicatedSystemMemory;
        public uint SharedSystemMemory;
        public _LUID AdapterLuid;
        public uint Flags;
    }

    public enum DXGI_ADAPTER_FLAG
    {
        DXGI_ADAPTER_FLAG_FORCE_DWORD = -1,
        DXGI_ADAPTER_FLAG_NONE = 0,
        DXGI_ADAPTER_FLAG_REMOTE = 1,
        DXGI_ADAPTER_FLAG_SOFTWARE = 2
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_DISPLAY_COLOR_SPACE
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        public float[] PrimaryCoordinates;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public float[] WhitePoints;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct DXGI_FRAME_STATISTICS
    {
        public uint PresentCount;
        public uint PresentRefreshCount;
        public uint SyncRefreshCount;
        public _LARGE_INTEGER SyncQPCTime;
        public _LARGE_INTEGER SyncGPUTime;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_GAMMA_CONTROL
    {
        public DXGI_RGB Scale;
        public DXGI_RGB Offset;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x401)]
        public DXGI_RGB[] GammaCurve;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_GAMMA_CONTROL_CAPABILITIES
    {
        public int ScaleAndOffsetSupported;
        public float MaxConvertedValue;
        public float MinConvertedValue;
        public uint NumGammaControlPoints;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x401)]
        public float[] ControlPointPositions;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_MAPPED_RECT
    {
        public int Pitch;

        public IntPtr pBits;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_MODE_DESC
    {
        public uint Width;
        public uint Height;
        public DXGI_RATIONAL RefreshRate;
        public DXGI_FORMAT Format;
        public DXGI_MODE_SCANLINE_ORDER ScanlineOrdering;
        public DXGI_MODE_SCALING Scaling;
    }

    public enum DXGI_MODE_ROTATION
    {
        DXGI_MODE_ROTATION_UNSPECIFIED,
        DXGI_MODE_ROTATION_IDENTITY,
        DXGI_MODE_ROTATION_ROTATE90,
        DXGI_MODE_ROTATION_ROTATE180,
        DXGI_MODE_ROTATION_ROTATE270
    }

    public enum DXGI_MODE_SCALING
    {
        DXGI_MODE_SCALING_UNSPECIFIED,
        DXGI_MODE_SCALING_CENTERED,
        DXGI_MODE_SCALING_STRETCHED
    }

    public enum DXGI_MODE_SCANLINE_ORDER
    {
        DXGI_MODE_SCANLINE_ORDER_UNSPECIFIED,
        DXGI_MODE_SCANLINE_ORDER_PROGRESSIVE,
        DXGI_MODE_SCANLINE_ORDER_UPPER_FIELD_FIRST,
        DXGI_MODE_SCANLINE_ORDER_LOWER_FIELD_FIRST
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_OUTPUT_DESC
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public ushort[] DeviceName;
        public tagRECT DesktopCoordinates;
        public int AttachedToDesktop;
        public DXGI_MODE_ROTATION Rotation;
        public IntPtr Monitor;
    }

    public enum DXGI_RESIDENCY
    {
        DXGI_RESIDENCY_EVICTED_TO_DISK = 3,
        DXGI_RESIDENCY_FULLY_RESIDENT = 1,
        DXGI_RESIDENCY_RESIDENT_IN_SHARED_MEMORY = 2
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_RGB
    {
        public float Red;
        public float Green;
        public float Blue;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_SHARED_RESOURCE
    {
        public IntPtr Handle;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_SURFACE_DESC
    {
        public uint Width;
        public uint Height;
        public DXGI_FORMAT Format;
        public DXGI_SAMPLE_DESC SampleDesc;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_SWAP_CHAIN_DESC
    {
        public DXGI_MODE_DESC BufferDesc;
        public DXGI_SAMPLE_DESC SampleDesc;
        public uint BufferUsage;
        public uint BufferCount;
        public IntPtr OutputWindow;
        public int Windowed;
        public DXGI_SWAP_EFFECT SwapEffect;
        public uint Flags;
    }

    public enum DXGI_SWAP_CHAIN_FLAG
    {
        DXGI_SWAP_CHAIN_FLAG_ALLOW_MODE_SWITCH = 2,
        DXGI_SWAP_CHAIN_FLAG_DIRECT_FLIP = 0x40,
        DXGI_SWAP_CHAIN_FLAG_DISPLAY_ONLY = 0x20,
        DXGI_SWAP_CHAIN_FLAG_GDI_COMPATIBLE = 4,
        DXGI_SWAP_CHAIN_FLAG_NONPREROTATED = 1,
        DXGI_SWAP_CHAIN_FLAG_RESTRICT_SHARED_RESOURCE_DRIVER = 0x10,
        DXGI_SWAP_CHAIN_FLAG_RESTRICTED_CONTENT = 8
    }

    public enum DXGI_SWAP_EFFECT
    {
        DXGI_SWAP_EFFECT_DISCARD = 0,
        DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL = 3,
        DXGI_SWAP_EFFECT_SEQUENTIAL = 1
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("2411E7E1-12AC-4CCF-BD14-9798E8534DC0")]
    public interface IDXGIAdapter : IDXGIObject
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void EnumOutputs([In] uint Output, [In, Out, MarshalAs(UnmanagedType.Interface)] ref IDXGIOutput ppOutput);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetDesc([MarshalAs(UnmanagedType.Struct)] ref DXGI_ADAPTER_DESC pDesc);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void CheckInterfaceSupport([In] ref Guid InterfaceName, out _LARGE_INTEGER pUMDVersion);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("29038F61-3839-4626-91FD-086879011A05")]
    public interface IDXGIAdapter1 : IDXGIAdapter
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void EnumOutputs([In] uint Output, [In, Out, MarshalAs(UnmanagedType.Interface)] ref IDXGIOutput ppOutput);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetDesc(out DXGI_ADAPTER_DESC pDesc);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void CheckInterfaceSupport([In] ref Guid InterfaceName, out _LARGE_INTEGER pUMDVersion);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetDesc1(out DXGI_ADAPTER_DESC1 pDesc);
    }

    [ComImport, Guid("54EC77FA-1377-44E6-8C32-88FD5F44C84C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGIDevice : IDXGIObject
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetAdapter([MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter pAdapter);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void CreateSurface([In] ref DXGI_SURFACE_DESC pDesc, [In] uint NumSurfaces, [In] uint Usage, [In] ref DXGI_SHARED_RESOURCE pSharedResource, [MarshalAs(UnmanagedType.Interface)] out IDXGISurface ppSurface);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void QueryResourceResidency([In, MarshalAs(UnmanagedType.IUnknown)] ref object ppResources, out DXGI_RESIDENCY pResidencyStatus, [In] uint NumResources);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetGPUThreadPriority([In] int Priority);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        int GetGPUThreadPriority();
    }

    [ComImport, Guid("77DB970F-6276-48BA-BA28-070143B4392C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGIDevice1 : IDXGIDevice
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetAdapter([MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter pAdapter);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void CreateSurface([In] ref DXGI_SURFACE_DESC pDesc, [In] uint NumSurfaces, [In] uint Usage, [In] ref DXGI_SHARED_RESOURCE pSharedResource, [MarshalAs(UnmanagedType.Interface)] out IDXGISurface ppSurface);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void QueryResourceResidency([In, MarshalAs(UnmanagedType.IUnknown)] ref object ppResources, out DXGI_RESIDENCY pResidencyStatus, [In] uint NumResources);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetGPUThreadPriority([In] int Priority);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        int GetGPUThreadPriority();
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetMaximumFrameLatency([In] uint MaxLatency);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetMaximumFrameLatency(out uint pMaxLatency);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("3D3E0379-F9DE-4D58-BB6C-18D62992F1A6")]
    public interface IDXGIDeviceSubObject : IDXGIObject
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetDevice([In] ref Guid riid);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("7B7166EC-21C7-44AE-B21A-C9AE321AE369")]
    public interface IDXGIFactory : IDXGIObject
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void EnumAdapters([In] uint Adapter, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter ppAdapter);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void MakeWindowAssociation(ref _RemotableHandle WindowHandle, uint Flags);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetWindowAssociation(out _RemotableHandle pWindowHandle);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void CreateSwapChain([In, MarshalAs(UnmanagedType.IUnknown)] object pDevice, [In] ref DXGI_SWAP_CHAIN_DESC pDesc, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain ppSwapChain);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void CreateSoftwareAdapter([In] IntPtr Module, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter ppAdapter);
    }

    [ComImport, Guid("770AAE78-F26F-4DBA-A829-253C83D1B387"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGIFactory1 : IDXGIFactory
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void EnumAdapters([In] uint Adapter, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter ppAdapter);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void MakeWindowAssociation(ref _RemotableHandle WindowHandle, uint Flags);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetWindowAssociation(out _RemotableHandle pWindowHandle);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void CreateSwapChain([In, MarshalAs(UnmanagedType.IUnknown)] object pDevice, [In] ref DXGI_SWAP_CHAIN_DESC pDesc, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain ppSwapChain);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void CreateSoftwareAdapter([In] IntPtr Module, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter ppAdapter);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void EnumAdapters1([In] uint Adapter, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter1 ppAdapter);
        [PreserveSig, MethodImpl(MethodImplOptions.PreserveSig)]
        int IsCurrent();
    }

    [ComImport, Guid("50C83A1C-E072-4C48-87B0-3630FA36A6D0"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGIFactory2 : IDXGIFactory1
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void EnumAdapters([In] uint Adapter, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter ppAdapter);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void MakeWindowAssociation(ref _RemotableHandle WindowHandle, uint Flags);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetWindowAssociation(out _RemotableHandle pWindowHandle);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void CreateSwapChain([In, MarshalAs(UnmanagedType.IUnknown)] object pDevice, [In] ref DXGI_SWAP_CHAIN_DESC pDesc, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain ppSwapChain);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void CreateSoftwareAdapter([In] IntPtr Module, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter ppAdapter);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void EnumAdapters1([In] uint Adapter, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter1 ppAdapter);
        [PreserveSig, MethodImpl(MethodImplOptions.PreserveSig)]
        int IsCurrent();
        [PreserveSig, MethodImpl(MethodImplOptions.PreserveSig)]
        int IsWindowedStereoEnabled();
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void CreateSwapChainForHwnd([In, MarshalAs(UnmanagedType.IUnknown)] object pDevice, [In] ref _RemotableHandle hWnd, [In] ref DXGI_SWAP_CHAIN_DESC1 pDesc, [In] ref DXGI_SWAP_CHAIN_FULLSCREEN_DESC pFullscreenDesc, [In, MarshalAs(UnmanagedType.Interface)] IDXGIOutput pRestrictToOutput, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain1 ppSwapChain);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void CreateSwapChainForCoreWindow([In, MarshalAs(UnmanagedType.IUnknown)] object pDevice, [In, MarshalAs(UnmanagedType.IUnknown)] object pWindow, [In] ref DXGI_SWAP_CHAIN_DESC1 pDesc, [In, MarshalAs(UnmanagedType.Interface)] IDXGIOutput pRestrictToOutput, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain1 ppSwapChain);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetSharedResourceAdapterLuid(IntPtr hResource, ref _LUID pLuid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void RegisterStereoStatusWindow([In] ref _RemotableHandle WindowHandle, [In] uint wMsg, out uint pdwCookie);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void RegisterStereoStatusEvent([In] IntPtr hEvent, out uint pdwCookie);
        [PreserveSig, MethodImpl(MethodImplOptions.PreserveSig)]
        void UnregisterStereoStatus([In] uint dwCookie);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void RegisterOcclusionStatusWindow([In] ref _RemotableHandle WindowHandle, [In] uint wMsg, out uint pdwCookie);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void RegisterOcclusionStatusEvent([In] IntPtr hEvent, out uint pdwCookie);
        [PreserveSig, MethodImpl(MethodImplOptions.PreserveSig)]
        void UnregisterOcclusionStatus([In] uint dwCookie);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void CreateSwapChainForComposition([In, MarshalAs(UnmanagedType.IUnknown)] object pDevice, [In] ref DXGI_SWAP_CHAIN_DESC1 pDesc, [In, MarshalAs(UnmanagedType.Interface)] IDXGIOutput pRestrictToOutput, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain1 ppSwapChain);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_SWAP_CHAIN_DESC1
    {
        public uint Width;
        public uint Height;
        public DXGI_FORMAT Format;
        public int Stereo;
        public DXGI_SAMPLE_DESC SampleDesc;
        public uint BufferUsage;
        public uint BufferCount;
        public DXGI_SCALING Scaling;
        public DXGI_SWAP_EFFECT SwapEffect;
        public DXGI_ALPHA_MODE AlphaMode;
        public uint Flags;
    }

    public enum DXGI_SCALING
    {
        DXGI_SCALING_STRETCH,
        DXGI_SCALING_NONE
    }

    public enum DXGI_ALPHA_MODE
    {
        DXGI_ALPHA_MODE_FORCE_DWORD = -1,
        DXGI_ALPHA_MODE_IGNORE = 3,
        DXGI_ALPHA_MODE_PREMULTIPLIED = 1,
        DXGI_ALPHA_MODE_STRAIGHT = 2,
        DXGI_ALPHA_MODE_UNSPECIFIED = 0
    }

    public enum DXGI_USAGE
    {
        DXGI_USAGE_SHADER_INPUT = (1 << (0 + 4)),
        DXGI_USAGE_RENDER_TARGET_OUTPUT = (1 << (1 + 4)),
        DXGI_USAGE_BACK_BUFFER = (1 << (2 + 4)),
        DXGI_USAGE_SHARED = (1 << (3 + 4)),
        DXGI_USAGE_READ_ONLY = (1 << (4 + 4)),
        DXGI_USAGE_DISCARD_ON_PRESENT = (1 << (5 + 4)),
        DXGI_USAGE_UNORDERED_ACCESS = (1 << (6 + 4)),
    }


    [ComImport, Guid("9D8E1289-D7B3-465F-8126-250E349AF85D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGIKeyedMutex : IDXGIDeviceSubObject
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetDevice([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void AcquireSync([In] ulong Key, [In] uint dwMilliseconds);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void ReleaseSync([In] ulong Key);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("AEC22FB8-76F3-4639-9BE0-28EB43A67A2E")]
    public interface IDXGIObject
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
    }

    [ComImport, Guid("AE02EEDB-C735-4690-8D52-5A8DC20213AA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGIOutput : IDXGIObject
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetDesc(out DXGI_OUTPUT_DESC pDesc);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetDisplayModeList([In] DXGI_FORMAT EnumFormat, [In] uint Flags, [In, Out] ref uint pNumModes, out DXGI_MODE_DESC pDesc);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void FindClosestMatchingMode([In] ref DXGI_MODE_DESC pModeToMatch, out DXGI_MODE_DESC pClosestMatch, [In, MarshalAs(UnmanagedType.IUnknown)] object pConcernedDevice);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void WaitForVBlank();
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void TakeOwnership([In, MarshalAs(UnmanagedType.IUnknown)] object pDevice, int Exclusive);
        [PreserveSig, MethodImpl(MethodImplOptions.PreserveSig)]
        void ReleaseOwnership();
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetGammaControlCapabilities(out DXGI_GAMMA_CONTROL_CAPABILITIES pGammaCaps);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetGammaControl([In] ref DXGI_GAMMA_CONTROL pArray);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetGammaControl(out DXGI_GAMMA_CONTROL pArray);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetDisplaySurface([In, MarshalAs(UnmanagedType.Interface)] IDXGISurface pScanoutSurface);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetDisplaySurfaceData([In, MarshalAs(UnmanagedType.Interface)] IDXGISurface pDestination);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetFrameStatistics(out DXGI_FRAME_STATISTICS pStats);
    }

    [ComImport, Guid("035F3AB4-482E-4E50-B41F-8A7F8BD8960B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGIResource : IDXGIDeviceSubObject
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetDevice([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetSharedHandle(out IntPtr pSharedHandle);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetUsage(out uint pUsage);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetEvictionPriority([In] uint EvictionPriority);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        uint GetEvictionPriority();
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("CAFCB56C-6AC3-4889-BF47-9E23BBD260EC")]
    public interface IDXGISurface : IDXGIDeviceSubObject
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetDevice([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetDesc(out DXGI_SURFACE_DESC pDesc);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void Map(out DXGI_MAPPED_RECT pLockedRect, [In] uint MapFlags);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void Unmap();
    }

    [ComImport, Guid("4AE63092-6327-4C1B-80AE-BFE12EA32B86"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGISurface1 : IDXGISurface
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetDevice([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetDesc(out DXGI_SURFACE_DESC pDesc);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void Map(out DXGI_MAPPED_RECT pLockedRect, [In] uint MapFlags);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void Unmap();
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetDC([In] int Discard, out _RemotableHandle phdc);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void ReleaseDC([In] ref tagRECT pDirtyRect);
    }

    [ComImport, Guid("310D36A0-D2E7-4C0A-AA04-6A9D23B8886A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGISwapChain : IDXGIDeviceSubObject
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.NoOptimization)]
        IntPtr GetDevice([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void Present([In] uint SyncInterval, [In] uint Flags);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetBuffer([In] uint Buffer, [In] ref Guid riid, [In, Out] ref IntPtr ppSurface);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetFullscreenState([In] int Fullscreen, [In, MarshalAs(UnmanagedType.Interface)] IDXGIOutput pTarget);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetFullscreenState(out int pFullscreen, [MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppTarget);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetDesc(out DXGI_SWAP_CHAIN_DESC pDesc);
        [MethodImpl(MethodImplOptions.NoOptimization)]
        void ResizeBuffers([In] uint BufferCount, [In] uint Width, [In] uint Height, [In] DXGI_FORMAT NewFormat, [In] uint SwapChainFlags);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void ResizeTarget([In] ref DXGI_MODE_DESC pNewTargetParameters);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetContainingOutput([MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppOutput);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetFrameStatistics(out DXGI_FRAME_STATISTICS pStats);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetLastPresentCount(out uint pLastPresentCount);
    }

    [ComImport, Guid("790A45F7-0D42-4876-983A-0A55CFE6F4AA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGISwapChain1 : IDXGISwapChain
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.NoOptimization)]
        IntPtr GetDevice([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void Present([In] uint SyncInterval, [In] uint Flags);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetBuffer([In] uint Buffer, [In] ref Guid riid, [In, Out] ref IntPtr ppSurface);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetFullscreenState([In] int Fullscreen, [In, MarshalAs(UnmanagedType.Interface)] IDXGIOutput pTarget);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetFullscreenState(out int pFullscreen, [MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppTarget);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetDesc(out DXGI_SWAP_CHAIN_DESC pDesc);
        [MethodImpl(MethodImplOptions.NoOptimization)]
        void ResizeBuffers([In] uint BufferCount, [In] uint Width, [In] uint Height, [In] DXGI_FORMAT NewFormat, [In] uint SwapChainFlags);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void ResizeTarget([In] ref DXGI_MODE_DESC pNewTargetParameters);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetContainingOutput([MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppOutput);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetFrameStatistics(out DXGI_FRAME_STATISTICS pStats);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetLastPresentCount(out uint pLastPresentCount);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetDesc1(out DXGI_SWAP_CHAIN_DESC1 pDesc);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetFullscreenDesc(out DXGI_SWAP_CHAIN_FULLSCREEN_DESC pDesc);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetHwnd(out _RemotableHandle pHwnd);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetCoreWindow([In] ref Guid refiid, out IntPtr ppUnk);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void Present1([In] uint SyncInterval, [In] uint PresentFlags, [In] ref DXGI_PRESENT_PARAMETERS pPresentParameters);
        [PreserveSig, MethodImpl(MethodImplOptions.PreserveSig)]
        int IsTemporaryMonoSupported();
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetRestrictToOutput([MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppRestrictToOutput);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetBackgroundColor([In] ref _D3DCOLORVALUE pColor);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetBackgroundColor(out _D3DCOLORVALUE pColor);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetRotation([In] DXGI_MODE_ROTATION Rotation);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetRotation(out DXGI_MODE_ROTATION pRotation);
    }

    [Guid("A8BE2AC4-199F-4946-B331-79599FB98DE7"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface IDXGISwapChain2 : IDXGISwapChain1
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetPrivateDataInterface([In] ref Guid Name, [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetParent([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.NoOptimization)]
        IntPtr GetDevice([In] ref Guid riid);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void Present([In] uint SyncInterval, [In] uint Flags);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetBuffer([In] uint Buffer, [In] ref Guid riid, [In, Out] ref IntPtr ppSurface);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetFullscreenState([In] int Fullscreen, [In, MarshalAs(UnmanagedType.Interface)] IDXGIOutput pTarget);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetFullscreenState(out int pFullscreen, [MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppTarget);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetDesc(out DXGI_SWAP_CHAIN_DESC pDesc);
        [MethodImpl(MethodImplOptions.NoOptimization)]
        void ResizeBuffers([In] uint BufferCount, [In] uint Width, [In] uint Height, [In] DXGI_FORMAT NewFormat, [In] uint SwapChainFlags);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void ResizeTarget([In] ref DXGI_MODE_DESC pNewTargetParameters);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetContainingOutput([MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppOutput);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetFrameStatistics(out DXGI_FRAME_STATISTICS pStats);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetLastPresentCount(out uint pLastPresentCount);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetDesc1(out DXGI_SWAP_CHAIN_DESC1 pDesc);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetFullscreenDesc(out DXGI_SWAP_CHAIN_FULLSCREEN_DESC pDesc);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetHwnd(out _RemotableHandle pHwnd);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetCoreWindow([In] ref Guid refiid, out IntPtr ppUnk);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void Present1([In] uint SyncInterval, [In] uint PresentFlags, [In] ref DXGI_PRESENT_PARAMETERS pPresentParameters);
        [PreserveSig, MethodImpl(MethodImplOptions.PreserveSig)]
        int IsTemporaryMonoSupported();
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetRestrictToOutput([MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppRestrictToOutput);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetBackgroundColor([In] ref _D3DCOLORVALUE pColor);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetBackgroundColor(out _D3DCOLORVALUE pColor);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetRotation([In] DXGI_MODE_ROTATION Rotation);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetRotation(out DXGI_MODE_ROTATION pRotation);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetSourceSize(uint Width, uint Height);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetSourceSize(out uint pWidth, out uint pHeight);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetMaximumFrameLatency(uint MaxLatency);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetMaximumFrameLatency(ref uint pMaxLatency);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        IntPtr GetFrameLatencyWaitableObject();
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetMatrixTransform(ref DXGI_MATRIX_3X2_F pMatrix);
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void GetMatrixTransform(ref DXGI_MATRIX_3X2_F pMatrix);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_MATRIX_3X2_F
    {
        public float _11;
        public float _12;
        public float _21;
        public float _22;
        public float _31;
        public float _32;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_SWAP_CHAIN_FULLSCREEN_DESC
    {
        public DXGI_RATIONAL RefreshRate;
        public DXGI_MODE_SCANLINE_ORDER ScanlineOrdering;
        public DXGI_MODE_SCALING Scaling;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_PRESENT_PARAMETERS
    {
        public uint DirtyRectsCount;
        public IntPtr pDirtyRects;
        public IntPtr pScrollRect;
        public IntPtr pScrollOffset;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct _D3DCOLORVALUE
    {
        public float r;
        public float g;
        public float b;
        public float a;
    }
}

