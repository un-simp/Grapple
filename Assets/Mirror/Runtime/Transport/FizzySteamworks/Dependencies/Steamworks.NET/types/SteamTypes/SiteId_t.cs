// This file is provided under The MIT License as part of Steamworks.NET.
// Copyright (c) 2013-2019 Riley Labrecque
// Please see the included LICENSE.txt for additional information.

// This file is automatically generated.
// Changes to this file will be reverted when you update Steamworks.NET

#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
	#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS

using System;

namespace Steamworks
{
    [Serializable]
    public struct SiteId_t : IEquatable<SiteId_t>, IComparable<SiteId_t>
    {
        public static readonly SiteId_t Invalid = new SiteId_t(0);
        public ulong m_SiteId;

        public SiteId_t(ulong value)
        {
            m_SiteId = value;
        }

        public int CompareTo(SiteId_t other)
        {
            return m_SiteId.CompareTo(other.m_SiteId);
        }

        public bool Equals(SiteId_t other)
        {
            return m_SiteId == other.m_SiteId;
        }

        public override string ToString()
        {
            return m_SiteId.ToString();
        }

        public override bool Equals(object other)
        {
            return other is SiteId_t && this == (SiteId_t) other;
        }

        public override int GetHashCode()
        {
            return m_SiteId.GetHashCode();
        }

        public static bool operator ==(SiteId_t x, SiteId_t y)
        {
            return x.m_SiteId == y.m_SiteId;
        }

        public static bool operator !=(SiteId_t x, SiteId_t y)
        {
            return !(x == y);
        }

        public static explicit operator SiteId_t(ulong value)
        {
            return new SiteId_t(value);
        }

        public static explicit operator ulong(SiteId_t that)
        {
            return that.m_SiteId;
        }
    }
}

#endif // !DISABLESTEAMWORKS