﻿/*
 * This file is subject to the terms and conditions defined in
 * file 'license.txt', which is part of this source code package.
 */



using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SteamKit2
{
    public class Blob
    {
        public List<BlobField> Fields { get; private set; }

        public BlobField this[ int descriptor ]
        {
            get { return LookupField( descriptor ); }
        }

        public BlobField this[ string descriptor ]
        {
            get { return LookupField( descriptor ); }
        }

        public ECacheState CacheState { get; set; }
        public EAutoPreprocessCode ProcessCode { get; set; }

        public int CompressionLevel { get; set; }
        public byte[] IV { get; set; }

        public byte[] Spare { get; set; }

        public Blob(ECacheState state, EAutoPreprocessCode code)
        {
            Fields = new List<BlobField>();

            CacheState = state;
            ProcessCode = code;
        }

        public void AddField( BlobField field )
        {
            Fields.Add( field );
        }

        private BlobField LookupField(string descriptor)
        {
            foreach ( BlobField field in Fields )
            {
                string fdesc = field.GetStringDescriptor();
                if ( fdesc != null && fdesc.Equals( descriptor, StringComparison.InvariantCultureIgnoreCase ) )
                {
                    return field;
                }
            }

            return null;
        }

        private BlobField LookupField(int descriptor)
        {
            foreach ( BlobField field in Fields )
            {
                if ( field.GetIntDescriptor() == descriptor )
                {
                    return field;
                }
            }

            return null;
        }


        public byte[] GetDescriptor( string descriptor )
        {
            BlobField field = LookupField( descriptor );

            if ( field == null || field.HasBlobChild() )
                return null;

            return field.Descriptor;
        }

        public byte[] GetDescriptor( int descriptor )
        {
            BlobField field = LookupField( descriptor );

            if ( field == null || field.HasBlobChild() )
                return null;

            return field.Data;
        }


        public string GetStringDescriptor( string descriptor )
        {
            BlobField field = LookupField( descriptor );

            if ( field == null || field.HasBlobChild() )
                return null;

            return field.GetStringData();
        }

        public string GetStringDescriptor( int descriptor )
        {
            BlobField field = LookupField( descriptor );

            if ( field == null || field.HasBlobChild() )
                return null;

            return field.GetStringData();
        }


        public Blob GetBlobDescriptor( string descriptor )
        {
            BlobField field = LookupField( descriptor );

            if ( field == null || !field.HasBlobChild() )
                return null;

            return field.GetChildBlob();
        }

        public Blob GetBlobDescriptor( int descriptor )
        {
            BlobField field = LookupField( descriptor );

            if ( field == null || !field.HasBlobChild() )
                return null;

            return field.GetChildBlob();
        }
    }

}
