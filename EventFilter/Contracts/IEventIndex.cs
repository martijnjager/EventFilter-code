﻿using System.Collections.Generic;

namespace EventFilter.Contracts
{
    public interface IEventIndex
    {
        int EventIdentifier { get; set; }

        //string[] Dates { get; set; }

        //string[] Description { get; set; }

        //string[] Id { get; set; }

        List<string> Events { get; set; }

        void IndexEvents();
        
        string Next(int curId);

        string Previous(int curId);
    }
}