using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventFilter.Test
{
    [TestClass()]
    public class BugsTest
    {
        [TestMethod()]
        public void CreateReportTest()
        {
            //Actions.Form = new Form1();
            //Bootstrap.Boot();

            //string debugText = "Encoding set toSystem.Text.SBCSCodePageEncoding \n  Load Keywords from G:\\Onedrive\\Projects\\EventFilter\\EventFilter\\bin\\Debug\\Keywords.txt \n Load event log from G:\\Onedrive\\Projects\\EventFilter\\EventFilter\\bin\\Debug\\eventlog.txt \n Initialization completed! \n Start searching events \n Selected log: Selected file: G:\\Onedrive\\Projects\\EventFilter\\EventFilter\\bin\\Debug\\eventlog.txt \n Parameters used: 	 filepath: G:\\Onedrive\\Projects\\EventFilter\\EventFilter\\bin\\Debug\\eventlog.txt \n      Keywords to use: Registry, recover, Reset, corrupt, disk, paging, registry, bad block, KMS, flush, IO, crash, dump, -No action, -shadow, -hive, -application, -Bluetooth, -Network Diag, -service name, -API, -Driver Management, -successful, -TCP \n Lines in eventArray: 17571757 \n  Events found: 10 \n Adding:   2017-10-09T01:19:20.910	The NetBIOS name and DNS host name of this machine have been changed from WIN-FMKOBU77TFV to DESKTOP-G0L88JA.   184 \n Adding:   2017-10-09T01:20:58.982	A service was installed in the system. \n Service Name:  AMD GPIO Client Driver \n Service File Name:  \\SystemRoot\\System32\\drivers\\amdgpio2.sys \n Service Type:  Kernelmodustreiber \n Service Start Type:  Manuell starten \n Service Account:	264 \n Adding:   2017-10-09T01:20:59.445	Installation Started: Windows has started installing the following update: Advanced Micro Devices, Inc driver update for AMD GPIO Controller    270 \n Adding:   2017-10-09T01:21:01.436	A service was installed in the system. \n Service Name:  AMD GPIO Client Driver \n Service File Name:  \\SystemRoot\\System32\\drivers\\amdgpio3.sys \n Service Type:  Kernelmodustreiber \n Service Start Type:  Manuell starten \n Service Account:	282 \n Adding:   2017-10-09T01:46:58.917	The system has rebooted without cleanly shutting down first.This error could be caused if the system stopped responding, crashed, or lost power unexpectedly.	481 \n Adding:   2017-10-09T02:08:04.633	The system has rebooted without cleanly shutting down first.This error could be caused if the system stopped responding, crashed, or lost power unexpectedly.	1307Adding:   2017-10-09T02:08:08.110	The computer has rebooted from a bugcheck.The bugcheck was: 0x0000001a (0x0000000000061941, 0x0000000073ffb0e0, 0x000000000000001d, 0xffffad00f6e25b00). A dump was saved in: C:\\Windows\\MEMORY.DMP.Report Id: cdf007bc-ab16-477d-9ed7-48fb02e9a528. 1330 \n Adding:   2017-10-10T19:05:50.372	Disk 3 has the same disk identifiers as one or more disks connected to the system.Go to Microsoft's support website (http://support.microsoft.com) and search for KB2983588 to resolve the issue.	1369 \n Adding:   2017-10-10T19:17:36.512	The system has rebooted without cleanly shutting down first.This error could be caused if the system stopped responding, crashed, or lost power unexpectedly.	1429 \nAdding:   2017-10-10T19:39:00.247	The system has rebooted without cleanly shutting down first.This error could be caused if the system stopped responding, crashed, or lost power unexpectedly.	1491";

            //Bug.CreateReport(debugText);

            //Assert.IsNull(Bug.Exception);
        }
    }
}
