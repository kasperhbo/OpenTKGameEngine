using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using ImGuiNET;
using MarioGabeKasper.Engine.Renderer;

namespace MarioGabeKasper.Engine.GUI;

public class ContentBrowserPanel
{
    private Directory currentDirectory = new Directory("", "Resources");
    private Texture directoryIcon;
    private Texture fileIcon;
    
    public ContentBrowserPanel()
    {
        directoryIcon = new Texture();
        directoryIcon.Init("EngineResources/Icons/DirectoryIcon.png", false);
        
        fileIcon = new Texture();
        fileIcon.Init("EngineResources/Icons/FileIcon.png");
    }
    
    private static GCHandle? _currentlyDraggedHandle;
    
    public unsafe void ImGui_()
    {
        bool currentlyDragging = false;
        DirectoryInfo directoryInfo = new DirectoryInfo(currentDirectory.Self);
        
        var files = directoryInfo.GetFiles().ToList();
        var folders = directoryInfo.GetDirectories().ToList();

        List<ContentBrowserItemInfo> cbInfo = new List<ContentBrowserItemInfo>();
        
        foreach (var f in files)
        {
            ContentBrowserItemInfo cInfo =
                new ContentBrowserItemInfo(FileType.File, new Directory(f.DirectoryName, f.Name));
            
            if (f.Name.Contains(".scene"))
                cInfo.FileType = FileType.Scene;
            
            cbInfo.Add(cInfo);
        }
        
        foreach (var f in folders)
        {
            ContentBrowserItemInfo cInfo =
                new ContentBrowserItemInfo(FileType.Folder, new Directory(f.Parent.Name, f.Name));
            
            cbInfo.Add(cInfo);
        }
        
        //Thanks the Cherno <3
        //Actual ImGui Code
        ImGui.Begin("Content Browser");
        
        //Back button
        if (directoryInfo != null && directoryInfo.Name != "Resources")
        {
            if (ImGui.Button("<- "  + currentDirectory.Self))
            {
                var tempSelf = currentDirectory.Self;
                currentDirectory.Self = currentDirectory.Parent;
                int pos = tempSelf.LastIndexOf("/");
                currentDirectory.Parent = currentDirectory.Parent.Remove(pos);
            }
        }
        
        float padding = 16.0f;
        float thumbnailSize = 128.0f;
        float cellSize = thumbnailSize + padding;

        float panelWidth = ImGui.GetContentRegionAvail().X;
        int columnCount = (int)(panelWidth / cellSize);
        if (columnCount < 1)
            columnCount = 1;

        ImGuiNET.ImGui.Columns(columnCount, "0", false);

        for (int i = 0; i < cbInfo.Count; i++)
        {
            ContentBrowserItemInfo cInfo = cbInfo[i];
            Texture icon = cInfo.FileType == FileType.Folder ? directoryIcon : fileIcon;
            
            ImGui.PushID(i);

            ImGui.ImageButton((IntPtr)icon.TexId, new Vector2(thumbnailSize, thumbnailSize));
            
            if(cInfo.FileType == FileType.File)
            {
                if (ImGui.BeginDragDropSource())
                {
                    currentlyDragging = true;
                    _currentlyDraggedHandle ??= GCHandle.Alloc(cInfo.DirectoryDate.Self);
                    ImGui.SetDragDropPayload("File_Drop", GCHandle.ToIntPtr(_currentlyDraggedHandle.Value),
                        (uint)sizeof(IntPtr));
                    ImGui.EndDragDropSource();
                }
            }
            if(cInfo.FileType == FileType.Scene)
            {
                if (ImGui.BeginDragDropSource())
                {
                    currentlyDragging = true;
                    _currentlyDraggedHandle ??= GCHandle.Alloc(cInfo.DirectoryDate.Self);
                    
                    ImGui.SetDragDropPayload("Scene_Drop", GCHandle.ToIntPtr(_currentlyDraggedHandle.Value),
                        (uint)sizeof(IntPtr));
                    
                    ImGui.EndDragDropSource();
                }
            }
            
            if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
            {
                if (cInfo.FileType == FileType.Folder || cInfo.FileType == FileType.Scene)
                {
                    var tempParent = currentDirectory.Self;
                    currentDirectory.Self = currentDirectory.Self + "/" + cInfo.DirectoryDate.Self;
                    currentDirectory.Parent = tempParent;
                }
            }
            
            ImGui.TextWrapped(cInfo.DirectoryDate.Self);
            ImGui.NextColumn();
            ImGui.PopID();
        }
        
        ImGui.Columns(1);
        
        //Todo status bar;
        
        
        ImGui.End();

        
        if (!currentlyDragging && _currentlyDraggedHandle != null)
        {
            _currentlyDraggedHandle.Value.Free();
            _currentlyDraggedHandle = null;
        }
    }

    private class ContentBrowserItemInfo
    {
        public FileType FileType { get; set; }
        public Directory DirectoryDate { get; }
        
        public ContentBrowserItemInfo(FileType fileType, Directory directoryDate)
        {
            this.FileType = fileType;
            this.DirectoryDate = directoryDate;
        }
    }
    
    struct Directory
    {
        public string Parent;
        public string Self;

        public Directory(string parent, string self)
        {
            Parent = parent;
            Self = self;
        }
    }
}



public enum FileType
{
    Folder,
    File,
    Shader,
    Scene,
    Sprite,
    Texture,
    TextFile,
}