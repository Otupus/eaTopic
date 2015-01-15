﻿//
//  EntityInfo.cs
//
//  Author:
//       Benito Palacios Sánchez <benito356@gmail.com>
//
//  Copyright (c) 2015 Benito Palacios Sánchez
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Linq;
using EaTopic.Topics;

namespace EaTopic
{
	internal abstract class EntityInfo : TopicData
	{
		public EntityInfo()
		{
			Uuid = Guid.NewGuid().ToByteArray();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != typeof(EntityInfo))
				return false;
			EntityInfo other = (EntityInfo)obj;
			return Uuid.SequenceEqual(other.Uuid);
		}

		public override int GetHashCode()
		{
			unchecked {
				return (Uuid != null ? Uuid.GetHashCode() : 0);
			}
		}

		public byte[] Uuid { get; protected set; }
	}
}

