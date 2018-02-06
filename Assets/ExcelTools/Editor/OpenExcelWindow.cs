using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEditor;
public class OpenExcelWindow : EditorWindow {
	
	public Vector2 scrollPosition = Vector2.zero;
	
	public static List<string> fileList = new List<string>();
	
	void Awake()
	{
		//title = "选择文件";
        titleContent.text = "选择文件";
	}
	
	[MenuItem("配置表工具/生成配置文件")]
	static void OpenXLSX(){
		fileList.Clear();
		
        string[] files = Directory.GetFiles(Application.dataPath + "/ExcelTools/xlsx/" );
		
		for(int i = 0 ; i < files.Length ; i++){
			string[] fileFolders = files[i].Split('/');
			
			string[] filenames = fileFolders[fileFolders.Length - 1].Split('.');
			
			if(filenames[filenames.Length - 1] == "xlsx"){
				string filename = fileFolders[fileFolders.Length - 1];
				fileList.Add(filename);
			}
		}
		
		Open();
	}
	
	void OnGUI()
	{
		scrollPosition = GUILayout.BeginScrollView(
			scrollPosition,
			GUILayout.Width(position.width),
			GUILayout.Height(position.height)
			);
		
		
		for(int i = 0 ;i < fileList.Count ; i ++){
			if (GUILayout.Button(fileList[i], GUILayout.Width(220), GUILayout.Height(32)))
			{
				SelectFile(i);
			}
		}
		GUILayout.EndScrollView();
		
	}
	
	void SelectFile(int index){
		string filestr = ExcelTool.LoadData(fileList[index]);
		EditorUtility.DisplayDialog("导出" + fileList[index]+ "成功！", filestr,  "确定");
	}
	
	public static OpenExcelWindow Open()
	{
		OpenExcelWindow window = (OpenExcelWindow)GetWindow<OpenExcelWindow>();
		window.Show();
		return window;
	}

}
