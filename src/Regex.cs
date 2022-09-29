using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

public partial class UDF {
  // By default the flags are 0 (ie. RegexOptions.None).
  // See https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regexoptions?view=net-6.0#system-text-regularexpressions-regexoptions-ignorecase
  // for description of all possible values.
  [Microsoft.SqlServer.Server.SqlFunction( IsDeterministic = true )]
  public static SqlString Match( string input, string pattern, int flags = 0 ) {
    if( string.IsNullOrEmpty( input ) || string.IsNullOrEmpty( pattern ) ) {
      return new SqlString( null );
    } else {
      var m = Regex.Match( input, pattern, (RegexOptions)flags );
      return new SqlString( m.Success ? m.Value : null );
    }
  }

  [Microsoft.SqlServer.Server.SqlFunction( IsDeterministic = true, IsPrecise = true )]
  public static SqlString GroupMatch( string input, string pattern, string group, int flags = 0 ) {
    if( string.IsNullOrEmpty( input ) || string.IsNullOrEmpty( pattern ) || string.IsNullOrEmpty( group ) ) {
      return new SqlString( null );
    } else {
      Group g = Regex.Match( input, pattern, (RegexOptions)flags ).Groups[ group ];

      return new SqlString( g.Success ? g.Value : null );
    }
  }

  [Microsoft.SqlServer.Server.SqlFunction( IsDeterministic = true, IsPrecise = true )]
  public static SqlString Replace( string input, string pattern, string replacement, int flags = 0 ) {
    // the replacement string is not checked for an empty string because that is a valid replacement pattern
    return string.IsNullOrEmpty( input ) || string.IsNullOrEmpty( pattern ) || replacement == null
      ? new SqlString( null )
      : new SqlString( Regex.Replace( input, pattern, replacement, (RegexOptions)flags ) );
  }

  [SqlFunction( DataAccess = DataAccessKind.None, FillRowMethodName = "FillMatches", TableDefinition = "Position int, MatchText nvarchar(max)" )]
  public static IEnumerable Matches( string input, string pattern, int flags = 0 ) {
    List<RegexMatch> MatchCollection = new();
    if( !string.IsNullOrEmpty( input ) && !string.IsNullOrEmpty( pattern ) ) {
      //only run through the matches if the inputs have non-empty, non-null strings
      foreach( Match m in Regex.Matches( input, pattern, (RegexOptions)flags ) ) {
        MatchCollection.Add( new RegexMatch( m.Index, m.Value ) );
      }
    }

    return MatchCollection;
  }

  [SqlFunction( DataAccess = DataAccessKind.None, FillRowMethodName = "FillMatches", TableDefinition = "Position int, MatchText nvarchar(max)" )]
  public static IEnumerable Split( string input, string pattern, int flags = 0 ) {
    List<RegexMatch> MatchCollection = new();
    if( !string.IsNullOrEmpty( input ) && !string.IsNullOrEmpty( pattern ) ) {
      //only run through the splits if the inputs have non-empty, non-null strings
      string[] splits = Regex.Split( input, pattern, (RegexOptions)flags );
      for( int i = 0; i < splits.Length; i++ ) {
        MatchCollection.Add( new RegexMatch( i, splits[ i ] ) );
      }
    }

    return MatchCollection;
  }

  public static void FillMatches( object match, out SqlInt32 Position, out SqlString MatchText ) {
    RegexMatch rm = (RegexMatch)match;
    Position = rm.Position;
    MatchText = rm.MatchText;
  }

  private class RegexMatch {
    public SqlInt32 Position { get; set; }
    public SqlString MatchText { get; set; }

    public RegexMatch( SqlInt32 position, SqlString match ) {
      this.Position = position;
      this.MatchText = match;
    }
  }
};

