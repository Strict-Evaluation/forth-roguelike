0 constant ATTR_OFF
1 constant ATTR_BOLD
4 constant ATTR_UNDERSCORE
5 constant ATTR_BLINK
7 constant ATTR_REVERSE
8 constant ATTR_CONCEALED

30 constant FG_BLACK
31 constant FG_RED
32 constant FG_GREEN
33 constant FG_YELLOW
34 constant FG_BLUE
35 constant FG_MAGENTA
36 constant FG_CYAN
37 constant FG_WHITE

40 constant BG_BLACK
41 constant BG_RED
42 constant BG_GREEN
43 constant BG_YELLOW
44 constant BG_BLUE
45 constant BG_MAGENTA
46 constant BG_CYAN
47 constant BG_WHITE

: prip s>d <# #S #> type ;

: ansi-escape-starter  ( - )  27 emit [char] [ emit ;
: ansi-seperator  ( - )  [char] ; emit ;
: set-attr  ( attr - )
	ansi-escape-starter prip [char] m emit ;
: attrs-off  ( - )
	ATTR_OFF set-attr ;
: set-attr-fg  ( fg attr - )
	ansi-escape-starter prip ansi-seperator prip [char] m emit ;
: graphics-normal!  ( - )
	ATTR_OFF FG_WHITE set-attr-fg ;
: set-graphics! ( bg fg attr )
	ansi-escape-starter prip ansi-seperator prip ansi-seperator prip [char] m emit ;

\ cursor movement
: move-cursor!  ( x y -- )
	ansi-escape-starter prip ansi-seperator prip [char] f emit ;

\ other display manipulation
: erase-display!  ( )
	ansi-escape-starter ." 2J"
	0 0 move-cursor! ;

: erase-line!
	ansi-escape-starter [char] K emit ;

\ Color triple things for easier colors
struct
	char% field graphics-attr
	char% field graphics-fg
	char% field graphics-bg
end-struct graphics%

: gtriples  ( i -- n )
	graphics% %size * ;

16 constant graphics%-count
create graphics%s graphics%-count gtriples allot
graphics%s graphics%-count gtriples 0 fill

: get-triple  ( i -- addr )
	gtriples graphics%s + ;
: set-triple!  ( bg fg attr addr  -- ) 
	2dup graphics-attr c! nip
	2dup graphics-fg c! nip
	graphics-bg c! ;
: init-color-triple! ( bg fg attr i -- )
	get-triple set-triple! ;
: set-color-by-graphics%  ( addr -- )
	dup graphics-bg c@ swap ( bg a )
	dup graphics-fg c@ swap ( bg fg a )
	graphics-attr c@ set-graphics! ;
: set-graphics-by-index!  ( i -- )
	get-triple set-color-by-graphics% ;
