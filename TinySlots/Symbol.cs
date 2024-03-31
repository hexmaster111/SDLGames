namespace TinySlots;

/**
 * Wheel one
 *      orange Grape cherry 2BAR Orange 7 Orange Bar Orange Grape 3BAR grape cherry Orange Bell bar GRAPE ORANGE GRAPE BAR
 * Wheel two
 *      orange bar bell cherry 2bar chary bell cherry Grape bell 2bar cherry 7 cherry Bell 3bar BELL cherry 2bar cherry
 * Wheel three
 *      orange grape bell 3bar orange bell orange bell orange 2bar grape bell orange grape orange bell orange bar orange 7
 *
 *  or = orange grape = grape bell = bell 2bar = 2bar 3bar = 3bar 7 = 7 bar = bar ch = cherry
 *            00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F 10 11 12 13
 *  wheel 1 : or gr ch 2b or  7 or  b or gr 3b gr ch or be  b gr or gr  b
 *  wheel 2 : or b  be ch 2b ch be ch gr be 2b ch  7 ch be 3b be ch 2b ch
 *  wheel 3 : or gr be 3b or be or be or 2b gr be or gr or be or  b or  7
 */

public static class WheelSymbolList
{
    //  *  wheel 1 : or gr ch 2b or  7 or  b or gr 3b gr ch or be  b gr or gr  b
    public static readonly Symbol[] Wheel1 = {
        Symbol.Orange, Symbol.Grape, Symbol.Cherry, Symbol.TwoBar, Symbol.Orange, Symbol.Seven, Symbol.Orange,
        Symbol.Bar, Symbol.Orange, Symbol.Grape, Symbol.ThreeBar, Symbol.Grape, Symbol.Cherry, Symbol.Orange,
        Symbol.Bell, Symbol.Bar, Symbol.Grape, Symbol.Orange, Symbol.Grape, Symbol.Bar
    };
    // *  wheel 2 : or b  be ch 2b ch be ch gr be 2b ch  7 ch be 3b be ch 2b ch
    public static readonly Symbol[] Wheel2 = {
        Symbol.Orange, Symbol.Bar, Symbol.Bell, Symbol.Cherry, Symbol.TwoBar, Symbol.Cherry, Symbol.Bell,
        Symbol.Cherry, Symbol.Grape, Symbol.Bell, Symbol.TwoBar, Symbol.Cherry, Symbol.Seven, Symbol.Cherry,
        Symbol.Bell, Symbol.ThreeBar, Symbol.Bell, Symbol.Cherry, Symbol.TwoBar, Symbol.Cherry
    };
    
    // *  wheel 3 : or gr be 3b or be or be or 2b gr be or gr or be or  b or  7
    public static readonly Symbol[] Wheel3 = {
        Symbol.Orange, Symbol.Grape, Symbol.Bell, Symbol.ThreeBar, Symbol.Orange, Symbol.Bell, Symbol.Orange,
        Symbol.Bell, Symbol.Orange, Symbol.TwoBar, Symbol.Grape, Symbol.Bell, Symbol.Orange, Symbol.Grape,
        Symbol.Orange, Symbol.Bell, Symbol.Orange, Symbol.Bar, Symbol.Orange, Symbol.Seven
    };
}


public enum Symbol
{
    Orange,
    Grape,
    Cherry,
    TwoBar,
    Bar,
    Bell,
    ThreeBar,
    Seven,
}