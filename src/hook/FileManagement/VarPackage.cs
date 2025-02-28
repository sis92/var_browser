﻿using ICSharpCode.SharpZipLib.Zip;
using SimpleJSON;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Runtime.InteropServices;
using Valve.Newtonsoft.Json;

namespace var_browser
{
    
    [System.Serializable]
	public class SerializableVarPackage
	{
        public List<string> FileEntryNames;
        public List<string> FileEntryLastWriteTimes;
        public List<long> FileEntrySizes;
		public List<string> RecursivePackageDependencies;


		public List<string> ClothingFileEntryNames;
        public List<string> ClothingTags;
        public List<string> HairFileEntryNames;
        public List<string> HairTags;

		public void Read(BinaryReader reader)
		{
            //FileEntryNames
            {
                var count = reader.ReadInt32();
                if (count > 0)
                {
                    FileEntryNames = new List<string>(count);
                    for (int i = 0; i < count; i++)
                    {
                        FileEntryNames.Add(reader.ReadString());
                    }
                }
            }
            //FileEntryLastWriteTimes
            {
                var count = reader.ReadInt32();
                if (count > 0)
                {
                    FileEntryLastWriteTimes = new List<string>(count);
                    for (int i = 0; i < count; i++)
                    {
                        FileEntryLastWriteTimes.Add(reader.ReadString());
                    }
                }
            }
            //FileEntrySizes
            {
                var count = reader.ReadInt32();
                if (count > 0)
                {
                    FileEntrySizes = new List<long>(count);
                    for (int i = 0; i < count; i++)
                    {
                        FileEntrySizes.Add(reader.ReadInt64());
                    }
                }
            }
            //RecursivePackageDependencies
            {
                var count = reader.ReadInt32();
                if (count > 0)
                {
                    RecursivePackageDependencies = new List<string>(count);
                    for (int i = 0; i < count; i++)
                    {
                        RecursivePackageDependencies.Add(reader.ReadString());
                    }
                }
            }

            //ClothingFileEntryNames
            {
                var count = reader.ReadInt32();
                if (count > 0)
                {
                    ClothingFileEntryNames = new List<string>(count);
                    for (int i = 0; i < count; i++)
                    {
                        ClothingFileEntryNames.Add(reader.ReadString());
                    }
                }
            }
            //ClothingTags
            {
                var count = reader.ReadInt32();
                if (count > 0)
                {
                    ClothingTags = new List<string>(count);
                    for (int i = 0; i < count; i++)
                    {
                        ClothingTags.Add(reader.ReadString());
                    }
                }
            }
            //HairFileEntryNames
            {
                var count = reader.ReadInt32();
                if (count > 0)
                {
                    HairFileEntryNames = new List<string>(count);
                    for (int i = 0; i < count; i++)
                    {
                        HairFileEntryNames.Add(reader.ReadString());
                    }
                }
            }
            //HairTags
            {
                var count = reader.ReadInt32();
                if (count > 0)
                {
                    HairTags = new List<string>(count);
                    for (int i = 0; i < count; i++)
                    {
                        HairTags.Add(reader.ReadString());
                    }
                }
            }
        }

		public void Write(BinaryWriter writer)
		{
            //FileEntryNames
            {
                var count = FileEntryNames?.Count ?? 0;
                writer.Write(count);
                if (count > 0)
                {
                    for (int i = 0; i < FileEntryNames.Count; i++)
                    {
						writer.Write(FileEntryNames[i]);
                    }
                }
            }
            //FileEntryLastWriteTimes
            {
                var count = FileEntryLastWriteTimes?.Count ?? 0;
                writer.Write(count);
                if (count > 0)
                {
                    for (int i = 0; i < FileEntryLastWriteTimes.Count; i++)
                    {
                        writer.Write(FileEntryLastWriteTimes[i]);
                    }
                }
            }
            //FileEntrySizes
            {
                var count = FileEntrySizes?.Count ?? 0;
                writer.Write(count);
                if (count > 0)
                {
                    for (int i = 0; i < FileEntrySizes.Count; i++)
                    {
                        writer.Write(FileEntrySizes[i]);
                    }
                }
            }
            //RecursivePackageDependencies
            {
                var count = RecursivePackageDependencies?.Count ?? 0;
                writer.Write(count);
                if (count > 0)
                {
                    for (int i = 0; i < RecursivePackageDependencies.Count; i++)
                    {
                        writer.Write(RecursivePackageDependencies[i]);
                    }
                }
            }

            //ClothingFileEntryNames
            {
                var count = ClothingFileEntryNames?.Count ?? 0;
                writer.Write(count);
                if (count > 0)
                {
                    for (int i = 0; i < ClothingFileEntryNames.Count; i++)
                    {
                        writer.Write(ClothingFileEntryNames[i]);
                    }
                }
            }
            //ClothingTags
            {
                var count = ClothingTags?.Count ?? 0;
                writer.Write(count);
                if (count > 0)
                {
                    for (int i = 0; i < ClothingTags.Count; i++)
                    {
                        writer.Write(ClothingTags[i]);
                    }
                }
            }
            //HairFileEntryNames
            {
                var count = HairFileEntryNames?.Count ?? 0;
                writer.Write(count);
                if (count > 0)
                {
                    for (int i = 0; i < HairFileEntryNames.Count; i++)
                    {
                        writer.Write(HairFileEntryNames[i]);
                    }
                }
            }
            //HairTags
            {
                var count = HairTags?.Count ?? 0;
                writer.Write(count);
                if (count > 0)
                {
                    for (int i = 0; i < HairTags.Count; i++)
                    {
                        writer.Write(HairTags[i]);
                    }
                }
            }
        }
    }

	public class VarPackage
	{
		public enum ReferenceVersionOption
		{
			Latest,
			Minimum,
			Exact
		}

		protected bool _enabled;
		public bool isNewestVersion;
		public bool isNewestEnabledVersion;
		protected string[] cacheFilePatterns = new string[2]
		{
			"*.vmi",
			"*.vam"
		};

		protected JSONClass jsonCache;

		protected VarFileEntry metaEntry;

		public bool invalid
		{
			get;
			protected set;
		}
		public bool IsCorruptedArchive = false;
		public bool Enabled
		{
			get
			{
				return true;
			}

		}

		public bool PluginsAlwaysEnabled
		{
			get
			{
				return true;
			}
		}

		public bool PluginsAlwaysDisabled
		{
			get
			{
				return false;
			}
		}

		public float packProgress
		{
			get;
			protected set;
		}

		public bool IsUnpacking
		{
			get
			{
				return false;
			}
		}

		public bool IsRepacking
		{
			get
			{
				return false;
			}
		}

		public bool HasOriginalCopy
		{
			get
			{
				string path = Path + ".orig";
				return FileManager.FileExists(path);
			}
		}

		// 解压之后的形式称为simulated
		public bool IsSimulated
		{
			get { return false; }
		}

		ZipFile m_ZipFile;
		public ZipFile ZipFile
		{
			get
			{
				if (m_ZipFile == null)
				{
					FileStream file = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete);
					m_ZipFile = new ZipFile(file);
					m_ZipFile.IsStreamOwner = true;
				}
				return m_ZipFile;
			}
			protected set
			{
				m_ZipFile = value;
			}
		}

		public string Uid
		{
			get;
			protected set;
		}

		string m_UidLowerInvariant;
		public string UidLowerInvariant
		{
			get
			{
				if (m_UidLowerInvariant == null)
				{
					m_UidLowerInvariant = this.Uid.ToLowerInvariant();
				}
				return m_UidLowerInvariant;
			}
		}

		public string Path
		{
			get;
			protected set;
		}
		public bool IsInstalled()
        {
			return Path.StartsWith("AddonPackages/");
		}

		public string RelativePath;
		public VarPackageGroup Group
		{
			get;
			protected set;
		}

		public string Name
		{
			get;
			protected set;
		}

		public int Version
		{
			get;
			protected set;
		}

		public ReferenceVersionOption StandardReferenceVersionOption
		{
			get;
			protected set;
		}

		public ReferenceVersionOption ScriptReferenceVersionOption
		{
			get;
			protected set;
		}

		public DateTime LastWriteTime
		{
			get;
			protected set;
		}
		public DateTime CreationTime
		{
			get;
			protected set;
		}

		public long Size
		{
			get;
			protected set;
		}

		public List<VarFileEntry> FileEntries
		{
			get;
			protected set;
		}
		public List<VarFileEntry> ClothingFileEntries
		{
			get;
			protected set;
		}
		public List<VarFileEntry> HairFileEntries
		{
			get;
			protected set;
		}
		public string Description
		{
			get;
			protected set;
		}

		public string Credits
		{
			get { return null; }
		}

		public string Instructions
		{
			get { return null; }
		}

		public string PromotionalLink
		{
			get;
			protected set;
		}
		public List<string> PackageDependencies
		{
			get;
			protected set;
		}

		//是否确认过所有的missing
		public bool MissingDependenciesChecked = false;
		public bool Scaned = false;
		public List<string> RecursivePackageDependencies;
		public List<string> ClothingFileEntryNames;
		public List<string> ClothingTags;
		public List<string> HairFileEntryNames;
		public List<string> HairTags;
		public bool HasMissingDependencies
		{
			get;
			protected set;
		}

		public HashSet<string> PackageDependenciesMissing
		{
			get;
			protected set;
		}

		public List<VarPackage> PackageDependenciesResolved
		{
			get;
			protected set;
		}

		public bool HadReferenceIssues
		{
			get;
			protected set;
		}
		public bool IsOnHub
		{
			get
			{
				if (HubBrowse.singleton != null)
				{
					string packageHubResourceId = HubBrowse.singleton.GetPackageHubResourceId(Uid);
					if (packageHubResourceId != null)
					{
						return true;
					}
				}
				return false;
			}
		}
		public string Creator
		{
			get;
			protected set;
		}
		public VarPackage(string uid, string path, VarPackageGroup group, string creator, string name, int version)
		{
			Uid = uid;//VAM_GS.Yinping_1_3.2 这种形式
			Path = path.Replace('\\', '/');//AllPackages/ReignMocap.RM-ActiveMaleSex.1.var 这种形式

			if (Path.StartsWith("AddonPackages/"))
				RelativePath = this.Path.Substring("AddonPackages/".Length);
			else if (Path.StartsWith("AllPackages/"))
				RelativePath = this.Path.Substring("AllPackages/".Length);
			else
				LogUtil.LogError("wrong path:"+Path);

			//Debug.Log("VarPackage " + Path+" "+ Uid+ " "+ name);
			Name = name;
			Group = group;
			//GroupName = group.Name;
			Creator = creator;

			Version = version;
			HadReferenceIssues = false;

			//PackageDependencies = new List<string>();
			//PackageDependenciesMissing = new HashSet<string>();
			//PackageDependenciesResolved = new List<VarPackage>();
			if (FileManager.debug)
			{
				//Debug.Log("New package\n Uid: " + Uid + "\n Path: " + Path + "\n FullPath: " + FullPath + "\n SlashPath: " + SlashPath + "\n Name: " + Name + "\n GroupName: " + GroupName + "\n Creator: " + Creator + "\n Version: " + Version);
			}
		}

		protected void SyncEnabled()
		{
			_enabled = true;// !FileManager.FileExists(Path + ".disabled");
		}

		public void Delete()
		{
			if (m_ZipFile != null)
			{
				m_ZipFile.Close();
				m_ZipFile = null;
			}
			if (File.Exists(Path))
			{
				FileManager.DeleteFile(Path);
			}
			else if (Directory.Exists(Path))
			{
				FileManager.DeleteDirectory(Path, true);
			}
			string path = Path + ".disabled";
			if (File.Exists(path))
			{
				FileManager.DeleteFile(path);
			}
			FileManager.Refresh();
		}

		public void RestoreFromOriginal()
		{
			string text = Path + ".orig";
			if (!FileManager.FileExists(text))
			{
				return;
			}
			if (FileManager.DirectoryExists(Path))
			{
				FileManager.DeleteDirectory(Path, true);
			}
			else if (FileManager.FileExists(Path))
			{
				if (m_ZipFile != null)
				{
					m_ZipFile.Close();
					m_ZipFile = null;
				}
				FileManager.DeleteFile(Path);
			}
			FileManager.MoveFile(text, Path);
		}
		public bool HasMatchingDirectories(string dir)
		{
			return false;
		}

		public void FindFiles(string dir, string pattern, List<FileEntry> foundFiles)
		{
			string pattern2 = "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
			foreach (VarFileEntry fileEntry in FileEntries)
			{
				if (fileEntry.InternalPath.StartsWith(dir) && Regex.IsMatch(fileEntry.Name, pattern2))
				{
					foundFiles.Add(fileEntry);
				}
			}
		}

		public void OpenOnHub()
		{
			if (var_browser.HubBrowse.singleton != null)
			{
				string packageHubResourceId = var_browser.HubBrowse.singleton.GetPackageHubResourceId(Uid);
				if (packageHubResourceId != null)
				{
					var_browser.HubBrowse.singleton.OpenDetail(packageHubResourceId);
				}
			}
		}

		public void Dispose()
		{
			if (m_ZipFile != null)
			{
				m_ZipFile.Close();
				m_ZipFile = null;
			}
		}

		public void SyncJSONCache()
		{
		}
		public void Scan()
		{
			if (Scaned) return;
			FileEntries = new List<VarFileEntry>();
			//ClothingFileEntries = new List<VarFileEntry>();
			//HairFileEntries = new List<VarFileEntry>();
			SyncEnabled();
			bool flag = false;
			if (File.Exists(Path))
			{
				ZipFile zipFile = null;
                try
                {
                    FileInfo fileInfo = new FileInfo(Path);
                    LastWriteTime = fileInfo.LastWriteTime;
					//new to old的排序按文件创建时间来排序
					CreationTime = fileInfo.CreationTime;
					Size = fileInfo.Length;

					metaEntry = null;
					string cacheJson = "Cache/AllPackagesJSON/" + this.Uid + ".json";

                    if (File.Exists(cacheJson) && new FileInfo(cacheJson).LastWriteTime >= fileInfo.LastWriteTime)
					{
                        SerializableVarPackage vp = VarPackageMgr.singleton.TryGetCache(this.Uid);
                        if (vp == null)
                        {
							string text = File.ReadAllText(cacheJson);
							vp = JsonUtility.FromJson<SerializableVarPackage>(text);
						}

						if (vp.FileEntryNames != null)
                        {
							this.ClothingFileEntryNames = vp.ClothingFileEntryNames;
							this.ClothingTags = vp.ClothingTags;
							this.HairFileEntryNames = vp.HairFileEntryNames;
							this.HairTags = vp.HairTags;

							for (int i=0;i<vp.FileEntryNames.Count;i++)
							{
								string item = vp.FileEntryNames[i];
								VarFileEntry varFileEntry = new VarFileEntry(this, item, DateTime.Parse(vp.FileEntryLastWriteTimes[i]), vp.FileEntrySizes[i]);
								FileEntries.Add(varFileEntry);
								if (item == "meta.json")
								{
									metaEntry = varFileEntry;
								}
							}
							this.RecursivePackageDependencies = vp.RecursivePackageDependencies;
						}

						if (metaEntry != null)
						{
							flag = true;
						}
					}
					else
					{
						FileStream file = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete);
						zipFile = new ZipFile(file);
						zipFile.IsStreamOwner = true;
						HashSet<string> set = new HashSet<string>();
						IEnumerator enumerator = zipFile.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								ZipEntry zipEntry = (ZipEntry)enumerator.Current;
								if (zipEntry.IsFile)
								{
									string entryName = zipEntry.Name;
									if (entryName.EndsWith(".json"))
									{
										if (zipEntry.Name == "meta.json")
										{
											VarFileEntry varFileEntry = new VarFileEntry(this, zipEntry.Name, zipEntry.DateTime, zipEntry.Size);
											FileEntries.Add(varFileEntry);
											if (zipEntry.Name == "meta.json")
											{
												metaEntry = varFileEntry;
											}
										}
										else
										{
											string entry = entryName.Substring(0, entryName.Length - 5) + ".jpg";
											if (!set.Contains(entry))
											{
												ZipEntry jpgEntry = zipFile.GetEntry(entry);
												if (jpgEntry != null)
												{
													VarFileEntry varFileEntry = new VarFileEntry(this, zipEntry.Name, zipEntry.DateTime, zipEntry.Size);
													FileEntries.Add(varFileEntry);
													VarFileEntry varFileEntry2 = new VarFileEntry(this, jpgEntry.Name, jpgEntry.DateTime, jpgEntry.Size);
													FileEntries.Add(varFileEntry2);

													set.Add(entry);

												}
											}
										}
									}
									else if (entryName.EndsWith(".vap"))
									{
										string entry = entryName.Substring(0, entryName.Length - 4) + ".jpg";
										if (!set.Contains(entry))
										{
											ZipEntry jpgEntry = zipFile.GetEntry(entry);
											if (jpgEntry != null)
											{
												VarFileEntry varFileEntry = new VarFileEntry(this, zipEntry.Name, zipEntry.DateTime, zipEntry.Size);
												FileEntries.Add(varFileEntry);
												VarFileEntry varFileEntry2 = new VarFileEntry(this, jpgEntry.Name, jpgEntry.DateTime, jpgEntry.Size);
												FileEntries.Add(varFileEntry2);
												set.Add(entry);
											}
										}
									}
									else if (entryName.EndsWith(".vam"))
									{
										string entry = entryName.Substring(0, entryName.Length - 4) + ".jpg";
										if (!set.Contains(entry))
										{
											ZipEntry jpgEntry = zipFile.GetEntry(entry);
											if (jpgEntry != null)
											{
												VarFileEntry varFileEntry = new VarFileEntry(this, zipEntry.Name, zipEntry.DateTime, zipEntry.Size);
												FileEntries.Add(varFileEntry);
												VarFileEntry varFileEntry2 = new VarFileEntry(this, jpgEntry.Name, jpgEntry.DateTime, jpgEntry.Size);
												FileEntries.Add(varFileEntry2);
												set.Add(entry);

												if(zipEntry.Name.StartsWith("Custom/Clothing/"))
                                                {
                                                    if (ClothingFileEntries == null)
                                                        ClothingFileEntries = new List<VarFileEntry>();
                                                    ClothingFileEntries.Add(varFileEntry);
												}
												if (zipEntry.Name.StartsWith("Custom/Hair/"))
												{
													if (HairFileEntries == null)
														HairFileEntries = new List<VarFileEntry>();
													HairFileEntries.Add(varFileEntry);
												}
											}
										}
									}
									//变形的数量太多了，2000多个包居然有8w多个变形
									//else if (entryName.EndsWith(".vmi"))
         //                           {
									//	VarFileEntry varFileEntry = new VarFileEntry(this, zipEntry.Name, zipEntry.DateTime, zipEntry.Size);
									//	FileEntries.Add(varFileEntry);
									//}
									else if (entryName.EndsWith(".assetbundle"))
									{
										VarFileEntry varFileEntry = new VarFileEntry(this, zipEntry.Name, zipEntry.DateTime, zipEntry.Size);
										FileEntries.Add(varFileEntry);
										//liu修改  增加资产预览图片
										string entry = entryName.Substring(0, entryName.Length - 12) + ".jpg";
										//SuperController.LogMessage("assetbundle:"+ entry);
										if (!set.Contains(entry))
										{
											ZipEntry jpgEntry = zipFile.GetEntry(entry);
											if (jpgEntry != null)
											{

												VarFileEntry varFileEntry2 = new VarFileEntry(this, jpgEntry.Name, jpgEntry.DateTime, jpgEntry.Size);
												FileEntries.Add(varFileEntry2);
												set.Add(entry);
											}
										}
									}
								}
							}
						}
						finally
						{
							IDisposable disposable;
							if ((disposable = (enumerator as IDisposable)) != null)
							{
								disposable.Dispose();
							}
						}

						if (metaEntry != null)
						{
							flag = true;
						}
						ZipFile = zipFile;
						DumpVarPackage();
					}
				}

                catch (Exception ex)
                {
                    zipFile?.Close();
					LogUtil.LogError("Exception during zip file scan of " + Path + ": " + ex);
                }
            }
			if (!flag)
			{
				invalid = true;
			}
			else
			{
				SyncJSONCache();
			}

            //初始化clothing tag
            if (ClothingFileEntryNames != null && ClothingFileEntryNames.Count > 0)
            {
				Dictionary<string, string> tags = new Dictionary<string, string>();
				for(int i = 0; i < ClothingFileEntryNames.Count; i++)
                {
					tags.Add(ClothingFileEntryNames[i], ClothingTags[i]);
				}
				foreach (var item in FileEntries)
				{
                    if (tags.ContainsKey(item.InternalPath))
                    {
						string tag = tags[item.InternalPath];
						string[] splits = tag.Split(',');

                        if (item.ClothingTags == null)
							item.ClothingTags = new List<string>();
                        else
							item.ClothingTags.Clear();

						for (int i = 0; i < splits.Length; i++)
                        {
							string t = splits[i].Trim();
							if(!string.IsNullOrEmpty(t))
                            {
								if (!TagFilter.AllClothingTags.Contains(t)&& !TagFilter.ClothingUnknownTags.Contains(t))
								{
									TagFilter.ClothingUnknownTags.Add(t);
									//LogUtil.Log("clothing tag " + t);
								}
								item.ClothingTags.Add(t);
							}
						}
					}
				}
			}

			if (HairFileEntryNames != null && HairFileEntryNames.Count > 0)
			{
				Dictionary<string, string> tags = new Dictionary<string, string>();
				for (int i = 0; i < HairFileEntryNames.Count; i++)
				{
					tags.Add(HairFileEntryNames[i], HairTags[i]);
				}
				foreach (var item in FileEntries)
				{
					if (tags.ContainsKey(item.InternalPath))
					{
						string tag = tags[item.InternalPath];
						string[] splits = tag.Split(',');

						if (item.HairTags == null)
							item.HairTags = new List<string>();
						else
							item.HairTags.Clear();

						for (int i = 0; i < splits.Length; i++)
						{
							string t = splits[i].Trim();
							if (!string.IsNullOrEmpty(t))
							{
								if (!TagFilter.AllHairTags.Contains(t)&& !TagFilter.HairUnknownTags.Contains(t))
								{
									TagFilter.HairUnknownTags.Add(t);
								//LogUtil.Log("hair tag " + t);
								}
								item.HairTags.Add(t);
							}
						}
					}
				}
			}
			Scaned = true;
		}
        //void FixVarName(string createName, string packageName)
        //{
        //    string uid = createName + "." + packageName + "." + Version;
			
        //    if (this.Uid != uid)
        //    {
        //        this.Uid = uid;
        //        fixUid = true;
        //    }
        //}
        void DumpVarPackage()
		{
			SerializableVarPackage svp = new SerializableVarPackage();
			List<string> clothingFileList = null;
			List<string> clothingTags = null;
            if (ClothingFileEntries != null)
            {
				clothingFileList = new List<string>();
				clothingTags = new List<string>();
				foreach (var item in ClothingFileEntries)
				{
					try
					{
						using (VarFileEntryStreamReader varFileEntryStreamReader = new VarFileEntryStreamReader(item))
						{
							string aJSON = varFileEntryStreamReader.ReadToEnd();
							JSONClass asObject = JSON.Parse(aJSON).AsObject;
							if (asObject != null)
							{
								if (asObject["tags"] != null)
								{
									string tag = asObject["tags"];
									tag = tag.Trim();
									if (!string.IsNullOrEmpty(tag))
									{
										clothingFileList.Add(item.InternalPath);
										clothingTags.Add(tag.ToLowerInvariant());
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						LogUtil.LogError("DumpVarPackage ClothingFileEntries " + ex.ToString());
					}
				}
			}

			List<string> hairFileList = null;
			List<string> hairTags = null;
            if (HairFileEntries != null)
            {
				hairFileList = new List<string>();
				hairTags = new List<string>();
				foreach (var item in HairFileEntries)
				{
					try
					{
						using (VarFileEntryStreamReader varFileEntryStreamReader = new VarFileEntryStreamReader(item))
						{
							string aJSON = varFileEntryStreamReader.ReadToEnd();
							JSONClass asObject = JSON.Parse(aJSON).AsObject;
							if (asObject != null)
							{
								if (asObject["tags"] != null)
								{
									string tag = asObject["tags"];
									tag = tag.Trim();
									if (!string.IsNullOrEmpty(tag))
									{
										hairFileList.Add(item.InternalPath);
										hairTags.Add(tag.ToLowerInvariant());
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						LogUtil.LogError("DumpVarPackage HairFileEntries " + ex.ToString());
					}
				}
			}


			if (metaEntry != null)
			{
				try
				{
					using (VarFileEntryStreamReader varFileEntryStreamReader = new VarFileEntryStreamReader(metaEntry))
					{
						string aJSON = varFileEntryStreamReader.ReadToEnd();
						JSONClass asObject = JSON.Parse(aJSON).AsObject;
						if (asObject != null)
						{
                            HashSet<string> depends = new HashSet<string>();
							//查找依赖
							GetDependenciesRecursive(asObject, depends);
							HashSet<string> scripts = new HashSet<string>();

							svp.RecursivePackageDependencies = depends.ToList();
						}
					}
				}
				catch (Exception ex3)
				{
					LogUtil.LogError("DumpVarPackage " + ex3.ToString());
				}
			}

			List<string> list1 = new List<string>();
			List<string> list2 = new List<string>();
			List<long> list3 = new List<long>();
			foreach (var item in FileEntries)
			{
				list1.Add(item.InternalPath);
				list2.Add(item.LastWriteTime.ToString());
				list3.Add(item.Size);
			}
			svp.FileEntryNames = list1;//.ToArray();
			svp.FileEntryLastWriteTimes = list2;//.ToArray();
			svp.FileEntrySizes = list3;//.ToArray();
			if (clothingFileList != null && clothingFileList.Count > 0)
                svp.ClothingFileEntryNames = clothingFileList;//.ToArray();
			if (clothingTags != null && clothingTags.Count > 0)
                svp.ClothingTags = clothingTags;//.ToArray();
			if (hairFileList != null && hairFileList.Count > 0)
                svp.HairFileEntryNames = hairFileList;//.ToArray();
			if (hairTags != null && hairTags.Count > 0)
                svp.HairTags = hairTags;//.ToArray();

			this.RecursivePackageDependencies = svp.RecursivePackageDependencies;
			this.ClothingFileEntryNames = svp.ClothingFileEntryNames;
			this.ClothingTags = svp.ClothingTags;
			this.HairFileEntryNames = svp.HairFileEntryNames;
			this.HairTags = svp.HairTags;

			string json = JsonUtility.ToJson(svp);

			string folder = "Cache/AllPackagesJSON";
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			File.WriteAllText(folder + "/" + this.Uid + ".json", json);
		}

		protected void FindMissingDependenciesRecursive(JSONClass jc)
		{
			JSONClass asObject = jc["dependencies"].AsObject;
			if (asObject != null)
			{
				foreach (string key in asObject.Keys)
				{
					VarPackage package = FileManager.GetPackage(key);
					if (package == null)
					{
						HasMissingDependencies = true;
						PackageDependenciesMissing.Add(key);
					}
					JSONClass asObject2 = asObject[key].AsObject;
					if (asObject2 != null)
					{
						FindMissingDependenciesRecursive(asObject2);
					}
				}
			}
		}
		void GetDependenciesRecursive(JSONClass jc, HashSet<string> depends)
		{
			JSONClass asObject = jc["dependencies"].AsObject;
			if (asObject != null)
			{
				foreach (string key in asObject.Keys)
				{
					depends.Add(key);
					JSONClass asObject2 = asObject[key].AsObject;
					if (asObject2 != null)
					{
						GetDependenciesRecursive(asObject2, depends);
					}
				}
			}
		}
		public bool InstallRecursive()
		{
			bool flag = false;
			bool dirty= InstallSelf();
			if (dirty) flag = true;
			
			//string linkvar = "AddonPackages/" + this.Uid + ".var";
            if (this.RecursivePackageDependencies != null)
            {
				foreach (var key in this.RecursivePackageDependencies)
				{
					VarPackage package = FileManager.GetPackage(key);
					if (package != null)
					{
						bool dirty2= package.InstallSelf();
						if (dirty2) flag = true;
					}
				}
			}
			if(flag)
				return true;
			return false;
		}
		public bool InstallSelf()
		{
			if (!this.Path.StartsWith("AllPackages/")) return false;

			string linkvar ="AddonPackages"+ this.Path.Substring("AllPackages".Length);
			if (File.Exists(linkvar)) return false;
			if (Directory.Exists(linkvar))//有可能存在同名文件夹
            {
				LogUtil.LogError("InstallSelf " + this.Path+" exist directory with same name");
				return false;
			}

			//移动文件
			string dir = System.IO.Path.GetDirectoryName(linkvar);
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			File.Move(this.Path, linkvar);
			FileInfo info = new FileInfo(linkvar);
			if (info != null)
				info.Refresh();
			return true;
		}
		public bool UninstallSelf()
        {
			if (!this.Path.StartsWith("AddonPackages/"))
            {
				LogUtil.LogError("Uninstall From AddonPackages "+this.Path);
				return false;
            }
			string linkvar = "AllPackages" + this.Path.Substring("AddonPackages".Length);
			if (File.Exists(linkvar))
            {
				LogUtil.LogError("Uninstall From AddonPackages Exists "+this.Path);
				return false;
			}
			//移动文件
			string dir = System.IO.Path.GetDirectoryName(linkvar);
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
			File.Move(this.Path, linkvar);
			FileInfo info = new FileInfo(linkvar);
            if (info != null)
                info.Refresh();
            return true;
		}
	}

}
