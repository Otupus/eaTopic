﻿//
//  Participant.cs
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
using System.Collections.Generic;
using System.Linq;
using EaTopic.Topics;
using EaTopic.Participants.Builtin;
using EaTopic.Publishers;
using EaTopic.Subscribers;

namespace EaTopic.Participants
{
	public class Participant : Entity
	{
		readonly List<Entity> topics;

		public Participant(int domain)
		{
			Domain = (byte)domain;
			topics = new List<Entity>();
			Info = new ParticipantInfo { Domain = this.Domain };
			BuiltinTopic = new BuiltinTopic(this);
		}

		public byte Domain {
			get;
			private set;
		}

		public BuiltinTopic BuiltinTopic { 
			get;
			private set; 
		}

		public override EntityInfo Info {
			get;
			set;
		}

		public override void Dispose()
		{
			foreach (var topic in topics)
				topic.Dispose();

			topics.Clear();
			BuiltinTopic.ForceUpdate();
		}

		public Topic<T> CreateTopic<T>(string name)
			where T : TopicData, new()
		{
			var topic = new Topic<T>(this, name, false);
			topics.Add(topic);
			return topic;
		}

		internal void UpdateInfo()
		{
			var partInfo = (ParticipantInfo)Info;
			partInfo.Topics = topics.Select(t => t.Info).Cast<TopicInfo>().ToArray();
			partInfo.Publishers  = topics
				.SelectMany(t => GetPublishers(t))
				.Select(pub => pub.Info)
				.Cast<PublisherInfo>()
				.ToArray();
			partInfo.Subscribers = topics
				.SelectMany(t => GetSubscribers(t))
				.Select(sub => sub.Info)
				.Cast<SubscriberInfo>()
				.ToArray();
			Info = partInfo;
		}

		internal IEnumerable<Entity> GetPublishers(Entity topic)
		{
			dynamic dynTopic = topic;
			return dynTopic.Publishers;
		}

		internal IEnumerable<Entity> GetSubscribers(Entity topic)
		{
			dynamic dynTopic = topic;
			return dynTopic.Subscribers;
		}
	}
}

