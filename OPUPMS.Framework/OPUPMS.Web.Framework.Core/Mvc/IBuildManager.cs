using System;
using System.Collections;
using System.IO;

namespace OPUPMS.Web.Framework.Core.Mvc
{
    internal interface IBuildManager
    {
        bool FileExists(string virtualPath);
        Type GetCompiledType(string virtualPath);
        ICollection GetReferencedAssemblies();
        Stream ReadCachedFile(string fileName);
        Stream CreateCachedFile(string fileName);
    }
}
