namespace Palantir.Health
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// A health indicator that checks the space on a disk.
	/// </summary>
	public sealed class DiskSpaceHealthIndicator : IHealthIndicator
	{
		public const long MEGABYTES = 1024 * 1024;
		public const long GIGABYTES = 1024 * MEGABYTES;

		public const long DefaultWarningThreshold = 15 * GIGABYTES;
		public const long DefaultErrorThreshold = 5 * GIGABYTES;
		public const string SystemDrive = @"C:\";
		private const string IndicatorName = "Disk Space";

		private readonly long warningThreshold;
		private readonly long errorThreshold;
		private readonly string drive;

		/// <summary>
		/// Initializes a new instance of the <see cref="DiskSpaceHealthIndicator"/> class.
		/// </summary>
		/// <param name="drive">The drive, e.g. "C:\".</param>
		/// <param name="warningThreshold">The warning threshold, in bytes.</param>
		/// <param name="errorThreshold">The error threshold, in bytes.</param>
		public DiskSpaceHealthIndicator(string drive = SystemDrive, long warningThreshold = DefaultWarningThreshold, long errorThreshold = DefaultErrorThreshold)
		{
			this.warningThreshold = warningThreshold;
			this.errorThreshold = errorThreshold;
			this.drive = drive;
		}

		/// <summary>
		/// The threshold (in bytes) below which the indicator will generate a warning.
		/// </summary>
		public long WarningThreshold => warningThreshold;

		/// <summary>
		/// The threshold (in bytes) below which the indicator will generate an error.
		/// </summary>
		public long ErrorThreshold => errorThreshold;

		/// <summary>
		/// The drive to check.
		/// </summary>
		public string Drive => drive;

		/// <summary>
		/// Peforms a health check.
		/// </summary>
		/// <returns>The health.</returns>
		public Task<Health> CheckHealthAsync()
		{
			var drive = DriveInfo.GetDrives().Where(x => x.Name == Drive).SingleOrDefault();
			if (drive == null)
				return Task.FromResult(new Health(IndicatorName, HealthStatus.Error, $"Drive {Drive} does not exist"));

			var available = drive.AvailableFreeSpace;
			var detail = new { Drive, TotalSize = GetSize(drive.TotalSize), AvailableFreeSpace = GetSize(available), ErrorThreshold = GetSize(ErrorThreshold), WarningThreshold = GetSize(WarningThreshold) };
			if (available < WarningThreshold)
				return Task.FromResult(new Health(IndicatorName, HealthStatus.Warning, $"Drive {Drive} has {GetSize(available)} free space available", detail));
			if (available < ErrorThreshold)
				return Task.FromResult(new Health(IndicatorName, HealthStatus.Error, $"Drive {Drive} has {GetSize(available)} free space available", detail));

			return Task.FromResult(new Health(IndicatorName, HealthStatus.OK, detail: detail));
		}

		/// <summary>
		/// Fetches the text representations of a set of bytes.
		/// </summary>
		/// <param name="size">The size in bytes.</param>
		/// <returns>The text representation of the size.</returns>
		private string GetSize(long size)
		{
			string postfix = "Bytes";
			long result = size;
			if (size >= 1073741824)//more than 1 GB
			{
				result = size / 1073741824;
				postfix = "GB";
			}
			else if (size >= 1048576)//more that 1 MB
			{
				result = size / 1048576;
				postfix = "MB";
			}
			else if (size >= 1024)//more that 1 KB
			{
				result = size / 1024;
				postfix = "KB";
			}

			return result.ToString("F1") + " " + postfix;
		}
	}
}
