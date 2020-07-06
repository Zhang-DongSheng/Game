﻿#region License
//Ntreev Photoshop Document Parser for .Net
//
//Released under the MIT License.
//
//Copyright (c) 2015 Ntreev Soft co., Ltd.
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//documentation files (the "Software"), to deal in the Software without restriction, including without limitation the 
//rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit 
//persons to whom the Software is furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the 
//Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
//WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
//OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System.Collections.Generic;
using SubjectNerd.PsdImporter.PsdParser.Readers.LayerAndMaskInformation;

namespace SubjectNerd.PsdImporter.PsdParser.Readers.LayerResources
{
    [ResourceID("lnkD")]
    class Reader_lnkD : ResourceReaderBase
    {
        public Reader_lnkD(PsdReader reader, long length)
            : base(reader, length)
        {
            
        }

        protected override void ReadValue(PsdReader reader, object userData, out IProperties value)
        {
            Properties props = new Properties();
            List<LinkedLayer> linkedLayers = new List<LinkedLayer>();
            while (reader.Position < this.EndPosition)
            {
                LinkedLayerReader r = new LinkedLayerReader(reader);
                linkedLayers.Add(r.Value);
            }

            props["Items"] = linkedLayers.ToArray();
            value = props;
        }
    }
}
