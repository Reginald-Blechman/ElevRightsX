using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using Microsoft.Win32;

namespace ElevRightsX
{
	// Token: 0x02000002 RID: 2
	public class TrustedBypass
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static string TrustedCrack(string args, bool visible)
		{
			ServiceController serviceController = new ServiceController
			{
				ServiceName = "TrustedInstaller"
			};
			try
			{
				bool flag = serviceController.StartType == ServiceStartMode.Disabled;
				if (flag)
				{
					Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\TrustedInstaller", true).SetValue("Start", 3);
				}
				bool flag2 = serviceController.Status != ServiceControllerStatus.Running;
				if (flag2)
				{
					serviceController.Start();
				}
			}
			catch
			{
			}
			Process[] processesByName = Process.GetProcessesByName("TrustedInstaller");
			return TrustedBypass.Run(processesByName[0].Id, args, visible);
		}

		// Token: 0x06000002 RID: 2
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, ref TrustedBypass.SECURITY_ATTRIBUTES lpProcessAttributes, ref TrustedBypass.SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, [In] ref TrustedBypass.STARTUPINFOEX lpStartupInfo, out TrustedBypass.PROCESS_INFORMATION lpProcessInformation);

		// Token: 0x06000003 RID: 3
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenProcess(TrustedBypass.ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

		// Token: 0x06000004 RID: 4
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern uint WaitForSingleObject(IntPtr handle, uint milliseconds);

		// Token: 0x06000005 RID: 5
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UpdateProcThreadAttribute(IntPtr lpAttributeList, uint dwFlags, IntPtr Attribute, IntPtr lpValue, IntPtr cbSize, IntPtr lpPreviousValue, IntPtr lpReturnSize);

		// Token: 0x06000006 RID: 6
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool InitializeProcThreadAttributeList(IntPtr lpAttributeList, int dwAttributeCount, int dwFlags, ref IntPtr lpSize);

		// Token: 0x06000007 RID: 7
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetHandleInformation(IntPtr hObject, TrustedBypass.HANDLE_FLAGS dwMask, TrustedBypass.HANDLE_FLAGS dwFlags);

		// Token: 0x06000008 RID: 8
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x06000009 RID: 9
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, IntPtr hSourceHandle, IntPtr hTargetProcessHandle, ref IntPtr lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

		// Token: 0x0600000A RID: 10 RVA: 0x000020F4 File Offset: 0x000002F4
		public static string Run(int parentProcessId, string binaryPath, bool visible)
		{
			TrustedBypass.PROCESS_INFORMATION process_INFORMATION = default(TrustedBypass.PROCESS_INFORMATION);
			TrustedBypass.STARTUPINFOEX startupinfoex = default(TrustedBypass.STARTUPINFOEX);
			IntPtr intPtr = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			TrustedBypass.InitializeProcThreadAttributeList(IntPtr.Zero, 1, 0, ref zero2);
			startupinfoex.lpAttributeList = Marshal.AllocHGlobal(zero2);
			TrustedBypass.InitializeProcThreadAttributeList(startupinfoex.lpAttributeList, 1, 0, ref zero2);
			IntPtr val = TrustedBypass.OpenProcess(TrustedBypass.ProcessAccessFlags.DuplicateHandle | TrustedBypass.ProcessAccessFlags.CreateProcess, false, parentProcessId);
			intPtr = Marshal.AllocHGlobal(IntPtr.Size);
			Marshal.WriteIntPtr(intPtr, val);
			TrustedBypass.UpdateProcThreadAttribute(startupinfoex.lpAttributeList, 0U, (IntPtr)131072, intPtr, (IntPtr)IntPtr.Size, IntPtr.Zero, IntPtr.Zero);
			TrustedBypass.SECURITY_ATTRIBUTES structure = default(TrustedBypass.SECURITY_ATTRIBUTES);
			TrustedBypass.SECURITY_ATTRIBUTES structure2 = default(TrustedBypass.SECURITY_ATTRIBUTES);
			structure.nLength = Marshal.SizeOf<TrustedBypass.SECURITY_ATTRIBUTES>(structure);
			structure2.nLength = Marshal.SizeOf<TrustedBypass.SECURITY_ATTRIBUTES>(structure2);
			bool flag = TrustedBypass.CreateProcess(null, binaryPath, ref structure, ref structure2, true, 524288U | (visible ? 16U : 134217728U), IntPtr.Zero, null, ref startupinfoex, out process_INFORMATION);
			return process_INFORMATION.dwProcessId.ToString();
		}

		// Token: 0x02000003 RID: 3
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct STARTUPINFOEX
		{
			// Token: 0x04000001 RID: 1
			public TrustedBypass.STARTUPINFO StartupInfo;

			// Token: 0x04000002 RID: 2
			public IntPtr lpAttributeList;
		}

		// Token: 0x02000004 RID: 4
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct STARTUPINFO
		{
			// Token: 0x04000003 RID: 3
			public int cb;

			// Token: 0x04000004 RID: 4
			public string lpReserved;

			// Token: 0x04000005 RID: 5
			public string lpDesktop;

			// Token: 0x04000006 RID: 6
			public string lpTitle;

			// Token: 0x04000007 RID: 7
			public int dwX;

			// Token: 0x04000008 RID: 8
			public int dwY;

			// Token: 0x04000009 RID: 9
			public int dwXSize;

			// Token: 0x0400000A RID: 10
			public int dwYSize;

			// Token: 0x0400000B RID: 11
			public int dwXCountChars;

			// Token: 0x0400000C RID: 12
			public int dwYCountChars;

			// Token: 0x0400000D RID: 13
			public int dwFillAttribute;

			// Token: 0x0400000E RID: 14
			public int dwFlags;

			// Token: 0x0400000F RID: 15
			public short wShowWindow;

			// Token: 0x04000010 RID: 16
			public short cbReserved2;

			// Token: 0x04000011 RID: 17
			public IntPtr lpReserved2;

			// Token: 0x04000012 RID: 18
			public IntPtr hStdInput;

			// Token: 0x04000013 RID: 19
			public IntPtr hStdOutput;

			// Token: 0x04000014 RID: 20
			public IntPtr hStdError;
		}

		// Token: 0x02000005 RID: 5
		internal struct PROCESS_INFORMATION
		{
			// Token: 0x04000015 RID: 21
			public IntPtr hProcess;

			// Token: 0x04000016 RID: 22
			public IntPtr hThread;

			// Token: 0x04000017 RID: 23
			public int dwProcessId;

			// Token: 0x04000018 RID: 24
			public int dwThreadId;
		}

		// Token: 0x02000006 RID: 6
		public struct SECURITY_ATTRIBUTES
		{
			// Token: 0x04000019 RID: 25
			public int nLength;

			// Token: 0x0400001A RID: 26
			public IntPtr lpSecurityDescriptor;

			// Token: 0x0400001B RID: 27
			[MarshalAs(UnmanagedType.Bool)]
			public bool bInheritHandle;
		}

		// Token: 0x02000007 RID: 7
		[Flags]
		public enum ProcessAccessFlags : uint
		{
			// Token: 0x0400001D RID: 29
			All = 2035711U,
			// Token: 0x0400001E RID: 30
			Terminate = 1U,
			// Token: 0x0400001F RID: 31
			CreateThread = 2U,
			// Token: 0x04000020 RID: 32
			VirtualMemoryOperation = 8U,
			// Token: 0x04000021 RID: 33
			VirtualMemoryRead = 16U,
			// Token: 0x04000022 RID: 34
			VirtualMemoryWrite = 32U,
			// Token: 0x04000023 RID: 35
			DuplicateHandle = 64U,
			// Token: 0x04000024 RID: 36
			CreateProcess = 128U,
			// Token: 0x04000025 RID: 37
			SetQuota = 256U,
			// Token: 0x04000026 RID: 38
			SetInformation = 512U,
			// Token: 0x04000027 RID: 39
			QueryInformation = 1024U,
			// Token: 0x04000028 RID: 40
			QueryLimitedInformation = 4096U,
			// Token: 0x04000029 RID: 41
			Synchronize = 1048576U
		}

		// Token: 0x02000008 RID: 8
		[Flags]
		private enum HANDLE_FLAGS : uint
		{
			// Token: 0x0400002B RID: 43
			None = 0U,
			// Token: 0x0400002C RID: 44
			INHERIT = 1U,
			// Token: 0x0400002D RID: 45
			PROTECT_FROM_CLOSE = 2U
		}
	}
}
