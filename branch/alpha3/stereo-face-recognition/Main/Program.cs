//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.GPU;
using System.Reflection;
using System.IO;
using FaceDetection.Forms;

namespace FaceDetection
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
       [STAThread]
       static void Main()
       {
           AppDomain.CurrentDomain.AssemblyResolve += FindAssem;

           Application.EnableVisualStyles();
           Application.SetCompatibleTextRenderingDefault(false);
           Application.Run(new FormMain());
       }

       static Assembly FindAssem(object sender, ResolveEventArgs args)
       {
           string assName = new AssemblyName(args.Name).Name;
           string openCvPath = System.Configuration.ConfigurationManager.AppSettings["OpenCVPath"] + assName + ".dll";

           if (File.Exists(openCvPath) == false)
           {
               return null;
           }

           return Assembly.LoadFrom(openCvPath);
       }
   }
}