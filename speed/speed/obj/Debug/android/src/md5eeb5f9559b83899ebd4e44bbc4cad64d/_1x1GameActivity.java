package md5eeb5f9559b83899ebd4e44bbc4cad64d;


public class _1x1GameActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("speed._1x1GameActivity, speed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", _1x1GameActivity.class, __md_methods);
	}


	public _1x1GameActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == _1x1GameActivity.class)
			mono.android.TypeManager.Activate ("speed._1x1GameActivity, speed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
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
