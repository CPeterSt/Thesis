package md55c9217afd300e2ebc94d968dfeebd8ba;


public class ChartObservableArrayList
	extends md55c9217afd300e2ebc94d968dfeebd8ba.ObservableArrayList
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Charts.ChartObservableArrayList, Syncfusion.SfChart.XForms.Android, Version=15.3451.0.29, Culture=neutral, PublicKeyToken=null", ChartObservableArrayList.class, __md_methods);
	}


	public ChartObservableArrayList () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ChartObservableArrayList.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.ChartObservableArrayList, Syncfusion.SfChart.XForms.Android, Version=15.3451.0.29, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
