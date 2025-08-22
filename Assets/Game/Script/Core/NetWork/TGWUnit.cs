using System;
using System.IO;
using System.Text;

public class TGWUnit
{
	private static bool m_bIsOpenTGW = false;
	public static bool OpenTGW
	{
        get { return m_bIsOpenTGW; }
	}
}
