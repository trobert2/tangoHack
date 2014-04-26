using UnityEngine;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class MemoryPinning : MonoBehaviour {

	private int numberOfItems = 100000;
	private Thread workingThread;
	private Work job;
	private string runStatistics;

	[StructLayout(LayoutKind.Sequential, Pack = 4)] 
	struct PinnedSubstructStructure{
		public int intVariable;
		public float floatVariable;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]                                                            
	struct PinnedStructure{
		public int intVariable;
		public float floatVariable;
		public byte byteType;
		public PinnedSubstructStructure subStruct;
	};

	class Work 
	{
		public bool testSucceed;
		public float populationTime;
		public float testTime;
		public string reason;

		private int mItems;
		private int mIValue;
		private float mFValue;
		private byte mBValue;

		private PinnedStructure[] arrayOfData;

		public Work(int ivalue, float fvalue, byte bvalue, int items)
		{
			mItems = items;
			mIValue = ivalue;
			mFValue = fvalue;
			mBValue = bvalue;
			arrayOfData = new PinnedStructure[items];
			reason = "";
		}

		public void PopupateDataFromNativeCode() 
		{
			GCHandle parray = GCHandle.Alloc(arrayOfData, GCHandleType.Pinned);

			System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
			watch.Start();
			Native.PopulateArray(mIValue, mFValue, mBValue, parray.AddrOfPinnedObject(), mItems);
			watch.Stop();
			populationTime = (float)watch.ElapsedMilliseconds/1000f;

			watch.Reset();
			watch.Start();
			testSucceed = TestData();
			watch.Stop();
			testTime = (float)watch.ElapsedMilliseconds/1000f;
			parray.Free();
			
		}

		private bool TestData()
		{
			for(int i=0; i<arrayOfData.Length; ++i)
			{
				if(   arrayOfData[i].intVariable != mIValue
				   || arrayOfData[i].floatVariable != mFValue
				   || arrayOfData[i].byteType != mBValue
				   || arrayOfData[i].subStruct.intVariable != mIValue
				   || arrayOfData[i].subStruct.floatVariable != mFValue
				   )
				{
					reason = "Iteration:"+ System.Convert.ToString(i)
							+" arrayOfData[i].intVariable="+  System.Convert.ToString(arrayOfData[i].intVariable)
							+" arrayOfData[i].floatVariable="+  System.Convert.ToString(arrayOfData[i].floatVariable)
							+" arrayOfData[i].byteType="+  System.Convert.ToString(arrayOfData[i].byteType)
							+" arrayOfData[i].subStruct.intVariable="+  System.Convert.ToString(arrayOfData[i].subStruct.intVariable)
							+" arrayOfData[i].subStruct.floatVariable="+  System.Convert.ToString(arrayOfData[i].subStruct.floatVariable)
							;
					return false;
				}
			}
			return true;
		}

		public struct Native
		{
			#if (UNITY_ANDROID)			
			[DllImport(Tango.Common.TangoUnityDLL)]
			public static extern void PopulateArray(int intSample, float floatSample, byte byteSample, System.IntPtr dataArray, int items);
			#else
			public static void PopulateArray(int intSample, float floatSample, byte byteSample, System.IntPtr dataArray, int items)
			{
				Thread.Sleep(200);
			}
			#endif
		}
	};

	void Start()
	{
		//looks like unity can't load android dynlib from inside of thread
		//so we access lib first time in main thread
		Work.Native.PopulateArray(0,0f,0,(System.IntPtr)null,0);
	}

	void Update(){
		if(workingThread != null 
		   && workingThread.ThreadState == ThreadState.Stopped)
		{
			runStatistics = "Data array populated. ";
			if(job.testSucceed){
				runStatistics += "No error detected ";
			}else{
				runStatistics += "Data test failed [";
				runStatistics += job.reason +"]";
			}

			runStatistics += " [Population time:"+job.populationTime+" sec]"
				+" [Test time:"+job.testTime+" sec]";

			workingThread = null;
			job = null;
		}
	}

	// Update is called once per frame
	void OnGUI () {
		numberOfItems = System.Convert.ToInt32( GUI.TextArea(new Rect(20, 20, 100, 60), System.Convert.ToString(numberOfItems)));
		GUI.Label(new Rect(20, 270, 400, 50), runStatistics);
		if (workingThread != null)
			return;

		if(GUI.Button(new Rect(20, 90, 300, 150), "Populate data array and validate it!"))
		{
			job = new Work(2, 3.5f, 33, numberOfItems);
			ThreadStart childref = new ThreadStart(job.PopupateDataFromNativeCode);
			workingThread = new Thread(childref);
			workingThread.Start();
			runStatistics = "Running...";
		}
	}
}
