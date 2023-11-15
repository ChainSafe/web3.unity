using System;
using System.Collections.Generic;

namespace ChainSafe.Gaming.UnityPackage.Model
{
    public class AllNftModel
    {
	    public class Metadata
	    {
		    public string description;
		    public string image;
		    public string name;
	    }

	    public class Root
	    {
		    public int page_number;
		    public int page_size;
		    public int total;
		    public string cursor;
		    public List<Token> tokens;
	    }

	    public class Token
	    {
		    public string token_id;
		    public string token_type;
		    public string contract_address;
		    public string supply;
		    public Metadata metadata;
	    }
    }
}