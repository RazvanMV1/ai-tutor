∑1
QD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.API\Controllers\AdminController.cs
	namespace		 	
AiTutor		
 
.		 
API		 
.		 
Controllers		 !
;		! "
[ 
ApiController 
] 
[ 
Route 
( 
$str 
) 
] 
[ 
AllowAnonymous 
] 
public 
class 
AdminController 
: 
ControllerBase -
{ 
private 
readonly  
ApplicationDbContext )
_db* -
;- .
private 
readonly 
UserManager  
<  !
ApplicationUser! 0
>0 1
_userManager2 >
;> ?
private 
readonly 
IConfiguration #
_config$ +
;+ ,
private 
readonly 
ILogger 
< 
AdminController ,
>, -
_logger. 5
;5 6
public 

AdminController 
(  
ApplicationDbContext 
db 
,  
UserManager 
< 
ApplicationUser #
># $
userManager% 0
,0 1
IConfiguration   
config   
,   
ILogger!! 
<!! 
AdminController!! 
>!!  
logger!!! '
)!!' (
{"" 
_db## 
=## 
db## 
;## 
_userManager$$ 
=$$ 
userManager$$ "
;$$" #
_config%% 
=%% 
config%% 
;%% 
_logger&& 
=&& 
logger&& 
;&& 
}'' 
[.. 
HttpPost.. 
(.. 
$str.. 
).. 
]..  
public// 

async// 
Task// 
<// 
IActionResult// #
>//# $
ResetDatabase//% 2
(//2 3
)//3 4
{00 
var11 
providedSecret11 
=11 
Request11 $
.11$ %
Headers11% ,
[11, -
$str11- =
]11= >
.11> ?
ToString11? G
(11G H
)11H I
;11I J
var22 
expectedSecret22 
=22 
_config22 $
[22$ %
$str22% >
]22> ?
;22? @
if44 

(44 
string44 
.44 
IsNullOrWhiteSpace44 %
(44% &
expectedSecret44& 4
)444 5
)445 6
{55 	
_logger66 
.66 

LogWarning66 
(66 
$str66 d
)66d e
;66e f
return77 

StatusCode77 
(77 
$num77 !
,77! "
new77# &
{77' (
error77) .
=77/ 0
$str771 W
}77X Y
)77Y Z
;77Z [
}88 	
if:: 

(:: 
!:: 
string:: 
.:: 
Equals:: 
(:: 
providedSecret:: )
,::) *
expectedSecret::+ 9
,::9 :
StringComparison::; K
.::K L
Ordinal::L S
)::S T
)::T U
{;; 	
_logger<< 
.<< 

LogWarning<< 
(<< 
$str<< J
)<<J K
;<<K L
return== 
Unauthorized== 
(==  
new==  #
{==$ %
error==& +
===, -
$str==. E
}==F G
)==G H
;==H I
}>> 	
_logger@@ 
.@@ 

LogWarning@@ 
(@@ 
$str@@ H
)@@H I
;@@I J
awaitCC 
_dbCC 
.CC 
DatabaseCC 
.CC 
EnsureDeletedAsyncCC -
(CC- .
)CC. /
;CC/ 0
awaitDD 
_dbDD 
.DD 
DatabaseDD 
.DD 
MigrateAsyncDD '
(DD' (
)DD( )
;DD) *
awaitEE 
SeedDataEE 
.EE 
	SeedAsyncEE  
(EE  !
_dbEE! $
,EE$ %
_userManagerEE& 2
)EE2 3
;EE3 4
_loggerGG 
.GG 
LogInformationGG 
(GG 
$strGG H
)GGH I
;GGI J
returnII 
OkII 
(II 
newII 
{JJ 	
messageKK 
=KK 
$strKK ?
,KK? @
	timestampLL 
=LL 
DateTimeLL  
.LL  !
UtcNowLL! '
}MM 	
)MM	 

;MM
 
}NN 
[TT 
HttpGetTT 
(TT 
$strTT 
)TT 
]TT 
publicUU 

asyncUU 
TaskUU 
<UU 
IActionResultUU #
>UU# $
HealthUU% +
(UU+ ,
)UU, -
{VV 
varWW 

canConnectWW 
=WW 
awaitWW 
_dbWW "
.WW" #
DatabaseWW# +
.WW+ ,
CanConnectAsyncWW, ;
(WW; <
)WW< =
;WW= >
varXX 
	userCountXX 
=XX 

canConnectXX "
?XX# $
awaitXX% *
_dbXX+ .
.XX. /
UsersXX/ 4
.XX4 5

CountAsyncXX5 ?
(XX? @
)XX@ A
:XXB C
$numXXD E
;XXE F
varYY 
identityUserCountYY 
=YY 

canConnectYY  *
?YY+ ,
awaitYY- 2
_userManagerYY3 ?
.YY? @
UsersYY@ E
.YYE F

CountAsyncYYF P
(YYP Q
)YYQ R
:YYS T
$numYYU V
;YYV W
return[[ 
Ok[[ 
([[ 
new[[ 
{\\ 	
dbReachable]] 
=]] 

canConnect]] $
,]]$ %
domainUsers^^ 
=^^ 
	userCount^^ #
,^^# $
identityUsers__ 
=__ 
identityUserCount__ -
,__- .
	timestamp`` 
=`` 
DateTime``  
.``  !
UtcNow``! '
}aa 	
)aa	 

;aa
 
}bb 
}cc ˆ[
ND:\Facultate\.NET\ai-tutor\src\backend\AiTutor.API\Controllers\AiController.cs
	namespace 	
AiTutor
 
. 
API 
. 
Controllers !
;! "
[ 
ApiController 
] 
[ 
Route 
( 
$str 
) 
] 
[ 
	Authorize 

]
 
public 
class 
AiController 
: 
ControllerBase *
{ 
private 
readonly 
	IMediator 
	_mediator (
;( )
private 
readonly 
IHttpClientFactory '
_httpClientFactory( :
;: ;
private 
readonly 
IConfiguration #
_configuration$ 2
;2 3
public 

AiController 
( 
	IMediator !
mediator" *
,* +
IHttpClientFactory, >
httpClientFactory? P
,P Q
IConfigurationR `
configurationa n
)n o
{ 
	_mediator 
= 
mediator 
; 
_httpClientFactory 
= 
httpClientFactory .
;. /
_configuration 
= 
configuration &
;& '
} 
public 

record 
ExplanationRequest $
($ %
string 
Topic 
, 
SubjectType 
Subject 
, 
DifficultyLevel   
DifficultyLevel   '
,  ' (
int!! 

StudentAge!! 
)"" 
;"" 
public$$ 

record$$ 
HintRequest$$ 
($$ 
string%% 
Question%% 
,%% 
SubjectType&& 
Subject&& 
)'' 
;'' 
public)) 

record)) 
ProblemsRequest)) !
())! "
string** 
Topic** 
,** 
SubjectType++ 
Subject++ 
,++ 
DifficultyLevel,, 
DifficultyLevel,, '
,,,' (
int-- 

StudentAge-- 
,-- 
int.. 
Count.. 
=.. 
$num.. 
)// 
;// 
public11 

record11 
ChatMessage11 
(11 
string11 $
Role11% )
,11) *
string11+ 1
Content112 9
)119 :
;11: ;
public33 

record33 
LessonChatRequest33 #
(33# $
string44 
LessonTitle44 
,44 
string55 
LessonContent55 
,55 
SubjectType66 
Subject66 
,66 
DifficultyLevel77 
DifficultyLevel77 '
,77' (
string88 
Question88 
,88 
List99 
<99 
ChatMessage99 
>99 
?99 
History99 "
):: 
;:: 
public<< 

record<< 
LessonChatResponse<< $
(<<$ %
string<<% +
Answer<<, 2
)<<2 3
;<<3 4
[?? 
HttpPost?? 
(?? 
$str?? 
)?? 
]?? 
public@@ 

async@@ 
Task@@ 
<@@ 
IActionResult@@ #
>@@# $
GetExplanation@@% 3
(@@3 4
[@@4 5
FromBody@@5 =
]@@= >
ExplanationRequest@@? Q
request@@R Y
)@@Y Z
{AA 
varBB 
resultBB 
=BB 
awaitBB 
	_mediatorBB $
.BB$ %
SendBB% )
(BB) *
newBB* -
GetExplanationQueryBB. A
(BBA B
requestCC 
.CC 
TopicCC 
,CC 
requestDD 
.DD 
SubjectDD 
,DD 
requestEE 
.EE 
DifficultyLevelEE #
,EE# $
requestFF 
.FF 

StudentAgeFF 
)GG 	
)GG	 

;GG
 
returnHH 
resultHH 
.HH 
	IsSuccessHH 
?HH  !
OkHH" $
(HH$ %
resultHH% +
.HH+ ,
DataHH, 0
)HH0 1
:HH2 3

BadRequestHH4 >
(HH> ?
resultHH? E
.HHE F
ErrorHHF K
)HHK L
;HHL M
}II 
[LL 
HttpPostLL 
(LL 
$strLL 
)LL 
]LL 
publicMM 

asyncMM 
TaskMM 
<MM 
IActionResultMM #
>MM# $
GetHintMM% ,
(MM, -
[MM- .
FromBodyMM. 6
]MM6 7
HintRequestMM8 C
requestMMD K
)MMK L
{NN 
varOO 
resultOO 
=OO 
awaitOO 
	_mediatorOO $
.OO$ %
SendOO% )
(OO) *
newOO* -
GetHintQueryOO. :
(OO: ;
requestPP 
.PP 
QuestionPP 
,PP 
requestQQ 
.QQ 
SubjectQQ 
)RR 	
)RR	 

;RR
 
returnSS 
resultSS 
.SS 
	IsSuccessSS 
?SS  !
OkSS" $
(SS$ %
resultSS% +
.SS+ ,
DataSS, 0
)SS0 1
:SS2 3

BadRequestSS4 >
(SS> ?
resultSS? E
.SSE F
ErrorSSF K
)SSK L
;SSL M
}TT 
[WW 
HttpPostWW 
(WW 
$strWW 
)WW 
]WW 
publicXX 

asyncXX 
TaskXX 
<XX 
IActionResultXX #
>XX# $
GenerateProblemsXX% 5
(XX5 6
[XX6 7
FromBodyXX7 ?
]XX? @
ProblemsRequestXXA P
requestXXQ X
)XXX Y
{YY 
varZZ 
resultZZ 
=ZZ 
awaitZZ 
	_mediatorZZ $
.ZZ$ %
SendZZ% )
(ZZ) *
newZZ* -!
GenerateProblemsQueryZZ. C
(ZZC D
request[[ 
.[[ 
Topic[[ 
,[[ 
request\\ 
.\\ 
Subject\\ 
,\\ 
request]] 
.]] 
DifficultyLevel]] #
,]]# $
request^^ 
.^^ 

StudentAge^^ 
,^^ 
request__ 
.__ 
Count__ 
)`` 	
)``	 

;``
 
returnaa 
resultaa 
.aa 
	IsSuccessaa 
?aa  !
Okaa" $
(aa$ %
resultaa% +
.aa+ ,
Dataaa, 0
)aa0 1
:aa2 3

BadRequestaa4 >
(aa> ?
resultaa? E
.aaE F
ErroraaF K
)aaK L
;aaL M
}bb 
[ee 
HttpPostee 
(ee 
$stree 
)ee 
]ee 
publicff 

asyncff 
Taskff 
<ff 
IActionResultff #
>ff# $

LessonChatff% /
(ff/ 0
[ff0 1
FromBodyff1 9
]ff9 :
LessonChatRequestff; L
requestffM T
)ffT U
{gg 
tryhh 
{ii 	
varjj 
	aiBaseUrljj 
=jj 
_configurationjj *
[jj* +
$strjj+ =
]jj= >
??jj? A
$strjjB Y
;jjY Z
varkk 
clientkk 
=kk 
_httpClientFactorykk +
.kk+ ,
CreateClientkk, 8
(kk8 9
)kk9 :
;kk: ;
clientll 
.ll 
Timeoutll 
=ll 
TimeSpanll %
.ll% &
FromSecondsll& 1
(ll1 2
$numll2 4
)ll4 5
;ll5 6
varoo 
payloadoo 
=oo 
newoo 
{pp 
lesson_titleqq 
=qq 
requestqq &
.qq& '
LessonTitleqq' 2
,qq2 3
lesson_contentrr 
=rr  
requestrr! (
.rr( )
LessonContentrr) 6
??rr7 9
stringrr: @
.rr@ A
EmptyrrA F
,rrF G
subjectss 
=ss 
(ss 
intss 
)ss 
requestss &
.ss& '
Subjectss' .
,ss. /
difficulty_leveltt  
=tt! "
(tt# $
inttt$ '
)tt' (
requesttt( /
.tt/ 0
DifficultyLeveltt0 ?
,tt? @
questionuu 
=uu 
requestuu "
.uu" #
Questionuu# +
,uu+ ,
historyvv 
=vv 
(vv 
requestvv "
.vv" #
Historyvv# *
??vv+ -
newvv. 1
Listvv2 6
<vv6 7
ChatMessagevv7 B
>vvB C
(vvC D
)vvD E
)vvE F
.ww 
Selectww 
(ww 
mww 
=>ww  
newww! $
{ww% &
roleww' +
=ww, -
mww. /
.ww/ 0
Roleww0 4
,ww4 5
contentww6 =
=ww> ?
mww@ A
.wwA B
ContentwwB I
}wwJ K
)wwK L
.xx 
ToListxx 
(xx 
)xx 
}yy 
;yy 
var{{ 
response{{ 
={{ 
await{{  
client{{! '
.{{' (
PostAsJsonAsync{{( 7
({{7 8
$"{{8 :
{{{: ;
	aiBaseUrl{{; D
}{{D E
$str{{E U
"{{U V
,{{V W
payload{{X _
){{_ `
;{{` a
var|| 
body|| 
=|| 
await|| 
response|| %
.||% &
Content||& -
.||- .
ReadAsStringAsync||. ?
(||? @
)||@ A
;||A B
if~~ 
(~~ 
!~~ 
response~~ 
.~~ 
IsSuccessStatusCode~~ -
)~~- .
return 

StatusCode !
(! "
(" #
int# &
)& '
response' /
./ 0

StatusCode0 :
,: ;
body< @
)@ A
;A B
using
ÅÅ 
var
ÅÅ 
doc
ÅÅ 
=
ÅÅ 
JsonDocument
ÅÅ (
.
ÅÅ( )
Parse
ÅÅ) .
(
ÅÅ. /
body
ÅÅ/ 3
)
ÅÅ3 4
;
ÅÅ4 5
var
ÇÇ 
answer
ÇÇ 
=
ÇÇ 
doc
ÇÇ 
.
ÇÇ 
RootElement
ÇÇ (
.
ÇÇ( )
TryGetProperty
ÇÇ) 7
(
ÇÇ7 8
$str
ÇÇ8 @
,
ÇÇ@ A
out
ÇÇB E
var
ÇÇF I
a
ÇÇJ K
)
ÇÇK L
?
ÇÇM N
a
ÇÇO P
.
ÇÇP Q
	GetString
ÇÇQ Z
(
ÇÇZ [
)
ÇÇ[ \
??
ÇÇ] _
$str
ÇÇ` b
:
ÇÇc d
$str
ÇÇe g
;
ÇÇg h
return
ÉÉ 
Ok
ÉÉ 
(
ÉÉ 
new
ÉÉ  
LessonChatResponse
ÉÉ ,
(
ÉÉ, -
answer
ÉÉ- 3
)
ÉÉ3 4
)
ÉÉ4 5
;
ÉÉ5 6
}
ÑÑ 	
catch
ÖÖ 
(
ÖÖ 
	Exception
ÖÖ 
ex
ÖÖ 
)
ÖÖ 
{
ÜÜ 	
return
áá 

StatusCode
áá 
(
áá 
$num
áá !
,
áá! "
new
áá# &
{
áá' (
error
áá) .
=
áá/ 0
ex
áá1 3
.
áá3 4
Message
áá4 ;
}
áá< =
)
áá= >
;
áá> ?
}
àà 	
}
ââ 
}ää é>
PD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.API\Controllers\AuthController.cs
	namespace 	
AiTutor
 
. 
API 
. 
Controllers !
;! "
public

 
class

 
AuthController

 
:

 
BaseController

 ,
{ 
private 
readonly 
UserManager  
<  !
ApplicationUser! 0
>0 1
_userManager2 >
;> ?
private 
readonly 
SignInManager "
<" #
ApplicationUser# 2
>2 3
_signInManager4 B
;B C
private 
readonly 
IJwtService  
_jwtService! ,
;, -
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 

AuthController 
( 
UserManager 
< 
ApplicationUser #
># $
userManager% 0
,0 1
SignInManager 
< 
ApplicationUser %
>% &
signInManager' 4
,4 5
IJwtService 

jwtService 
, !
IApplicationDbContext 
context %
)% &
{ 
_userManager 
= 
userManager "
;" #
_signInManager 
= 
signInManager &
;& '
_jwtService 
= 

jwtService  
;  !
_context 
= 
context 
; 
} 
[ 
HttpPost 
( 
$str 
) 
] 
public 

async 
Task 
< 
IActionResult #
># $
Register% -
(- .
[. /
FromBody/ 7
]7 8
RegisterRequest9 H
requestI P
)P Q
{ 
var   
result   
=   
await   
Mediator   #
.  # $
Send  $ (
(  ( )
new  ) ,
CreateUserCommand  - >
(  > ?
request!! 
.!! 
	FirstName!! 
,!! 
request"" 
."" 
LastName"" 
,"" 
request## 
.## 
Email## 
,## 
request$$ 
.$$ 
Password$$ 
,$$ 
request%% 
.%% 
Role%% 
)%% 
)%% 
;%% 
if'' 

('' 
!'' 
result'' 
.'' 
	IsSuccess'' 
)'' 
return(( 

StatusCode(( 
((( 
result(( $
.(($ %

StatusCode((% /
,((/ 0
new((1 4
{((5 6
message((7 >
=((? @
result((A G
.((G H
Error((H M
}((N O
)((O P
;((P Q
var** 
identityUser** 
=** 
new** 
ApplicationUser** .
{++ 	
UserName,, 
=,, 
request,, 
.,, 
Email,, $
,,,$ %
Email-- 
=-- 
request-- 
.-- 
Email-- !
,--! "
DomainUserId.. 
=.. 
result.. !
...! "
Data.." &
!..& '
...' (
Id..( *
}// 	
;//	 

var11 
identityResult11 
=11 
await11 "
_userManager11# /
.11/ 0
CreateAsync110 ;
(11; <
identityUser11< H
,11H I
request11J Q
.11Q R
Password11R Z
)11Z [
;11[ \
if22 

(22 
!22 
identityResult22 
.22 
	Succeeded22 %
)22% &
return33 

BadRequest33 
(33 
identityResult33 ,
.33, -
Errors33- 3
)333 4
;334 5
return55 

StatusCode55 
(55 
$num55 
,55 
result55 %
.55% &
Data55& *
)55* +
;55+ ,
}66 
[88 
HttpPost88 
(88 
$str88 
)88 
]88 
public99 

async99 
Task99 
<99 
IActionResult99 #
>99# $
Login99% *
(99* +
[99+ ,
FromBody99, 4
]994 5
LoginRequest996 B
request99C J
)99J K
{:: 
var;; 
identityUser;; 
=;; 
await;;  
_userManager;;! -
.;;- .
FindByEmailAsync;;. >
(;;> ?
request;;? F
.;;F G
Email;;G L
);;L M
;;;M N
if<< 

(<< 
identityUser<< 
is<< 
null<<  
)<<  !
return== 
Unauthorized== 
(==  
new==  #
{==$ %
message==& -
===. /
$str==0 F
}==G H
)==H I
;==I J
var?? 
result?? 
=?? 
await?? 
_signInManager?? )
.@@ $
CheckPasswordSignInAsync@@ %
(@@% &
identityUser@@& 2
,@@2 3
request@@4 ;
.@@; <
Password@@< D
,@@D E
false@@F K
)@@K L
;@@L M
ifBB 

(BB 
!BB 
resultBB 
.BB 
	SucceededBB 
)BB 
returnCC 
UnauthorizedCC 
(CC  
newCC  #
{CC$ %
messageCC& -
=CC. /
$strCC0 F
}CCG H
)CCH I
;CCI J
varEE 

domainUserEE 
=EE 
_contextEE !
.EE! "
UsersEE" '
.FF 
FirstOrDefaultFF 
(FF 
uFF 
=>FF  
uFF! "
.FF" #
IdFF# %
==FF& (
identityUserFF) 5
.FF5 6
DomainUserIdFF6 B
)FFB C
;FFC D
ifHH 

(HH 

domainUserHH 
isHH 
nullHH 
)HH 
returnII 
UnauthorizedII 
(II  
newII  #
{II$ %
messageII& -
=II. /
$strII0 A
}IIB C
)IIC D
;IID E
varKK 
tokenKK 
=KK 
_jwtServiceKK 
.KK  
GenerateTokenKK  -
(KK- .

domainUserKK. 8
)KK8 9
;KK9 :
returnMM 
OkMM 
(MM 
newMM 
{NN 	
tokenOO 
,OO 
userPP 
=PP 
newPP 
{QQ 

domainUserRR 
.RR 
IdRR 
,RR 

domainUserSS 
.SS 
FullNameSS #
,SS# $
EmailTT 
=TT 

domainUserTT "
.TT" #
EmailTT# (
.TT( )
ValueTT) .
,TT. /

domainUserUU 
.UU 
RoleUU 
}VV 
}WW 	
)WW	 

;WW
 
}XX 
}YY 
public[[ 
record[[ 
RegisterRequest[[ 
([[ 
string\\ 

	FirstName\\ 
,\\ 
string]] 

LastName]] 
,]] 
string^^ 

Email^^ 
,^^ 
string__ 

Password__ 
,__ 
UserRole`` 
Role`` 
)`` 
;`` 
publicbb 
recordbb 
LoginRequestbb 
(bb 
stringbb !
Emailbb" '
,bb' (
stringbb) /
Passwordbb0 8
)bb8 9
;bb9 :≤
PD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.API\Controllers\BaseController.cs
	namespace 	
AiTutor
 
. 
API 
. 
Controllers !
;! "
[ 
ApiController 
] 
[ 
Route 
( 
$str 
) 
] 
public 
abstract 
class 
BaseController $
:% &
ControllerBase' 5
{		 
private

 
ISender

 
?

 
	_mediator

 
;

 
	protected 
ISender 
Mediator 
=> !
	_mediator 
??= 
HttpContext !
.! "
RequestServices" 1
.1 2
GetRequiredService2 D
<D E
ISenderE L
>L M
(M N
)N O
;O P
} ˚ƒ
VD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.API\Controllers\ClassroomsController.cs
	namespace!! 	
AiTutor!!
 
.!! 
API!! 
.!! 
Controllers!! !
;!!! "
[## 
ApiController## 
]## 
[$$ 
Route$$ 
($$ 
$str$$ 
)$$ 
]$$ 
[%% 
	Authorize%% 

]%%
 
public&& 
class&&  
ClassroomsController&& !
:&&" #
ControllerBase&&$ 2
{'' 
private(( 
readonly(( 
	IMediator(( 
	_mediator(( (
;((( )
public)) 
 
ClassroomsController)) 
())  
	IMediator))  )
mediator))* 2
)))2 3
=>))4 6
	_mediator))7 @
=))A B
mediator))C K
;))K L
public,, 

record,, "
CreateClassroomRequest,, (
(,,( )
string,,) /
Name,,0 4
,,,4 5
string,,6 <
Description,,= H
,,,H I
SubjectType,,J U
SubjectType,,V a
),,a b
;,,b c
public-- 

record-- "
UpdateClassroomRequest-- (
(--( )
string--) /
Name--0 4
,--4 5
string--6 <
?--< =
Description--> I
)--I J
;--J K
public.. 

record..  
JoinClassroomRequest.. &
(..& '
string..' -
	ClassCode... 7
,..7 8
Guid..9 =
	StudentId..> G
)..G H
;..H I
public// 

record// 
AddMemberRequest// "
(//" #
string//# )
StudentEmail//* 6
)//6 7
;//7 8
public00 

record00 
CreateLessonRequest00 %
(00% &
string00& ,
Title00- 2
,002 3
string004 :
Content00; B
,00B C
int00D G

OrderIndex00H R
,00R S
DifficultyLevel00T c

Difficulty00d n
)00n o
;00o p
public11 

record11 
CreateQuizRequest11 #
(11# $
string11$ *
Title11+ 0
,110 1
DifficultyLevel112 A

Difficulty11B L
,11L M
int11N Q
TimeLimitMinutes11R b
)11b c
;11c d
public22 

record22 
AddQuestionRequest22 $
(22$ %
string22% +
Text22, 0
,220 1
string222 8
CorrectAnswer229 F
,22F G
List22H L
<22L M
string22M S
>22S T
Options22U \
,22\ ]
int22^ a
Points22b h
,22h i
string22j p
?22p q
Explanation22r }
)22} ~
;22~ 
public33 

record33 
SubmitQuizRequest33 #
(33# $
List33$ (
<33( )
QuizAnswerDto33) 6
>336 7
Answers338 ?
)33? @
;33@ A
public44 

record44 
AddGradeRequest44 !
(44! "
Guid44" &
	StudentId44' 0
,440 1
int442 5
Value446 ;
,44; <
string44= C
?44C D
Description44E P
)44P Q
;44Q R
public55 

record55 
UpdateGradeRequest55 $
(55$ %
int55% (
Value55) .
,55. /
string550 6
?556 7
Description558 C
)55C D
;55D E
[88 
HttpGet88 
(88 
$str88 
)88 
]88 
public99 

async99 
Task99 
<99 
IActionResult99 #
>99# $
GetMyClassrooms99% 4
(994 5
[995 6
	FromQuery996 ?
]99? @
Guid99A E
userId99F L
)99L M
{:: 
var;; 
result;; 
=;; 
await;; 
	_mediator;; $
.;;$ %
Send;;% )
(;;) *
new;;* - 
GetMyClassroomsQuery;;. B
(;;B C
userId;;C I
);;I J
);;J K
;;;K L
return<< 
result<< 
.<< 
	IsSuccess<< 
?<<  !
Ok<<" $
(<<$ %
result<<% +
.<<+ ,
Data<<, 0
)<<0 1
:<<2 3

BadRequest<<4 >
(<<> ?
result<<? E
.<<E F
Error<<F K
)<<K L
;<<L M
}== 
[?? 
HttpGet?? 
(?? 
$str?? 
)?? 
]?? 
public@@ 

async@@ 
Task@@ 
<@@ 
IActionResult@@ #
>@@# $
GetById@@% ,
(@@, -
Guid@@- 1
id@@2 4
,@@4 5
[@@6 7
	FromQuery@@7 @
]@@@ A
Guid@@B F
userId@@G M
)@@M N
{AA 
varBB 
resultBB 
=BB 
awaitBB 
	_mediatorBB $
.BB$ %
SendBB% )
(BB) *
newBB* -!
GetClassroomByIdQueryBB. C
(BBC D
idBBD F
,BBF G
userIdBBH N
)BBN O
)BBO P
;BBP Q
returnCC 
resultCC 
.CC 
	IsSuccessCC 
?CC  !
OkCC" $
(CC$ %
resultCC% +
.CC+ ,
DataCC, 0
)CC0 1
:CC2 3
NotFoundCC4 <
(CC< =
resultCC= C
.CCC D
ErrorCCD I
)CCI J
;CCJ K
}DD 
[FF 
HttpPostFF 
]FF 
publicGG 

asyncGG 
TaskGG 
<GG 
IActionResultGG #
>GG# $
CreateGG% +
(GG+ ,
[GG, -
FromBodyGG- 5
]GG5 6"
CreateClassroomRequestGG7 M
requestGGN U
,GGU V
[GGW X
	FromQueryGGX a
]GGa b
GuidGGc g
	teacherIdGGh q
)GGq r
{HH 
varII 
resultII 
=II 
awaitII 
	_mediatorII $
.II$ %
SendII% )
(II) *
newII* -"
CreateClassroomCommandII. D
(IID E
requestIIE L
.IIL M
NameIIM Q
,IIQ R
requestIIS Z
.IIZ [
DescriptionII[ f
,IIf g
requestIIh o
.IIo p
SubjectTypeIIp {
,II{ |
	teacherId	II} Ü
)
IIÜ á
)
IIá à
;
IIà â
returnJJ 
resultJJ 
.JJ 
	IsSuccessJJ 
?JJ  !

StatusCodeJJ" ,
(JJ, -
resultJJ- 3
.JJ3 4

StatusCodeJJ4 >
,JJ> ?
resultJJ@ F
.JJF G
DataJJG K
)JJK L
:JJM N

BadRequestJJO Y
(JJY Z
resultJJZ `
.JJ` a
ErrorJJa f
)JJf g
;JJg h
}KK 
[MM 
HttpPutMM 
(MM 
$strMM 
)MM 
]MM 
publicNN 

asyncNN 
TaskNN 
<NN 
IActionResultNN #
>NN# $
UpdateNN% +
(NN+ ,
GuidNN, 0
idNN1 3
,NN3 4
[NN5 6
FromBodyNN6 >
]NN> ?"
UpdateClassroomRequestNN@ V
requestNNW ^
,NN^ _
[NN` a
	FromQueryNNa j
]NNj k
GuidNNl p
	teacherIdNNq z
)NNz {
{OO 
varPP 
resultPP 
=PP 
awaitPP 
	_mediatorPP $
.PP$ %
SendPP% )
(PP) *
newPP* -"
UpdateClassroomCommandPP. D
(PPD E
idPPE G
,PPG H
	teacherIdPPI R
,PPR S
requestPPT [
.PP[ \
NamePP\ `
,PP` a
requestPPb i
.PPi j
DescriptionPPj u
)PPu v
)PPv w
;PPw x
returnQQ 
resultQQ 
.QQ 
	IsSuccessQQ 
?QQ  !
OkQQ" $
(QQ$ %
resultQQ% +
.QQ+ ,
DataQQ, 0
)QQ0 1
:QQ2 3

BadRequestQQ4 >
(QQ> ?
resultQQ? E
.QQE F
ErrorQQF K
)QQK L
;QQL M
}RR 
[TT 

HttpDeleteTT 
(TT 
$strTT 
)TT 
]TT 
publicUU 

asyncUU 
TaskUU 
<UU 
IActionResultUU #
>UU# $
DeleteUU% +
(UU+ ,
GuidUU, 0
idUU1 3
,UU3 4
[UU5 6
	FromQueryUU6 ?
]UU? @
GuidUUA E
	teacherIdUUF O
)UUO P
{VV 
varWW 
resultWW 
=WW 
awaitWW 
	_mediatorWW $
.WW$ %
SendWW% )
(WW) *
newWW* -"
DeleteClassroomCommandWW. D
(WWD E
idWWE G
,WWG H
	teacherIdWWI R
)WWR S
)WWS T
;WWT U
returnXX 
resultXX 
.XX 
	IsSuccessXX 
?XX  !
OkXX" $
(XX$ %
)XX% &
:XX' (

BadRequestXX) 3
(XX3 4
resultXX4 :
.XX: ;
ErrorXX; @
)XX@ A
;XXA B
}YY 
[\\ 
HttpPost\\ 
(\\ 
$str\\ 
)\\ 
]\\ 
public]] 

async]] 
Task]] 
<]] 
IActionResult]] #
>]]# $
Join]]% )
(]]) *
[]]* +
FromBody]]+ 3
]]]3 4 
JoinClassroomRequest]]5 I
request]]J Q
)]]Q R
{^^ 
var__ 
result__ 
=__ 
await__ 
	_mediator__ $
.__$ %
Send__% )
(__) *
new__* - 
JoinClassroomCommand__. B
(__B C
request__C J
.__J K
	ClassCode__K T
,__T U
request__V ]
.__] ^
	StudentId__^ g
)__g h
)__h i
;__i j
return`` 
result`` 
.`` 
	IsSuccess`` 
?``  !
Ok``" $
(``$ %
)``% &
:``' (

BadRequest``) 3
(``3 4
result``4 :
.``: ;
Error``; @
)``@ A
;``A B
}aa 
[cc 
HttpGetcc 
(cc 
$strcc 
)cc 
]cc 
publicdd 

asyncdd 
Taskdd 
<dd 
IActionResultdd #
>dd# $

GetMembersdd% /
(dd/ 0
Guiddd0 4
iddd5 7
,dd7 8
[dd9 :
	FromQuerydd: C
]ddC D
GuidddE I
	teacherIdddJ S
)ddS T
{ee 
varff 
resultff 
=ff 
awaitff 
	_mediatorff $
.ff$ %
Sendff% )
(ff) *
newff* -$
GetClassroomMembersQueryff. F
(ffF G
idffG I
,ffI J
	teacherIdffK T
)ffT U
)ffU V
;ffV W
returngg 
resultgg 
.gg 
	IsSuccessgg 
?gg  !
Okgg" $
(gg$ %
resultgg% +
.gg+ ,
Datagg, 0
)gg0 1
:gg2 3

BadRequestgg4 >
(gg> ?
resultgg? E
.ggE F
ErrorggF K
)ggK L
;ggL M
}hh 
[jj 
HttpPostjj 
(jj 
$strjj 
)jj 
]jj 
publickk 

asynckk 
Taskkk 
<kk 
IActionResultkk #
>kk# $
	AddMemberkk% .
(kk. /
Guidkk/ 3
idkk4 6
,kk6 7
[kk8 9
FromBodykk9 A
]kkA B
AddMemberRequestkkC S
requestkkT [
,kk[ \
[kk] ^
	FromQuerykk^ g
]kkg h
Guidkki m
	teacherIdkkn w
)kkw x
{ll 
varmm 
resultmm 
=mm 
awaitmm 
	_mediatormm $
.mm$ %
Sendmm% )
(mm) *
newmm* -
AddMemberCommandmm. >
(mm> ?
idmm? A
,mmA B
	teacherIdmmC L
,mmL M
requestmmN U
.mmU V
StudentEmailmmV b
)mmb c
)mmc d
;mmd e
returnnn 
resultnn 
.nn 
	IsSuccessnn 
?nn  !
Oknn" $
(nn$ %
)nn% &
:nn' (

BadRequestnn) 3
(nn3 4
resultnn4 :
.nn: ;
Errornn; @
)nn@ A
;nnA B
}oo 
[qq 

HttpDeleteqq 
(qq 
$strqq *
)qq* +
]qq+ ,
publicrr 

asyncrr 
Taskrr 
<rr 
IActionResultrr #
>rr# $
RemoveMemberrr% 1
(rr1 2
Guidrr2 6
idrr7 9
,rr9 :
Guidrr; ?
	studentIdrr@ I
,rrI J
[rrK L
	FromQueryrrL U
]rrU V
GuidrrW [
	teacherIdrr\ e
)rre f
{ss 
vartt 
resulttt 
=tt 
awaittt 
	_mediatortt $
.tt$ %
Sendtt% )
(tt) *
newtt* -
RemoveMemberCommandtt. A
(ttA B
idttB D
,ttD E
	teacherIdttF O
,ttO P
	studentIdttQ Z
)ttZ [
)tt[ \
;tt\ ]
returnuu 
resultuu 
.uu 
	IsSuccessuu 
?uu  !
Okuu" $
(uu$ %
)uu% &
:uu' (

BadRequestuu) 3
(uu3 4
resultuu4 :
.uu: ;
Erroruu; @
)uu@ A
;uuA B
}vv 
[yy 
HttpGetyy 
(yy 
$stryy 
)yy 
]yy 
publiczz 

asynczz 
Taskzz 
<zz 
IActionResultzz #
>zz# $

GetLessonszz% /
(zz/ 0
Guidzz0 4
idzz5 7
,zz7 8
[zz9 :
	FromQueryzz: C
]zzC D
GuidzzE I
userIdzzJ P
)zzP Q
{{{ 
var|| 
result|| 
=|| 
await|| 
	_mediator|| $
.||$ %
Send||% )
(||) *
new||* -$
GetClassroomLessonsQuery||. F
(||F G
id||G I
,||I J
userId||K Q
)||Q R
)||R S
;||S T
return}} 
result}} 
.}} 
	IsSuccess}} 
?}}  !
Ok}}" $
(}}$ %
result}}% +
.}}+ ,
Data}}, 0
)}}0 1
:}}2 3

BadRequest}}4 >
(}}> ?
result}}? E
.}}E F
Error}}F K
)}}K L
;}}L M
}~~ 
[
ÄÄ 
HttpPost
ÄÄ 
(
ÄÄ 
$str
ÄÄ 
)
ÄÄ 
]
ÄÄ 
public
ÅÅ 

async
ÅÅ 
Task
ÅÅ 
<
ÅÅ 
IActionResult
ÅÅ #
>
ÅÅ# $
CreateLesson
ÅÅ% 1
(
ÅÅ1 2
Guid
ÅÅ2 6
id
ÅÅ7 9
,
ÅÅ9 :
[
ÅÅ; <
FromBody
ÅÅ< D
]
ÅÅD E!
CreateLessonRequest
ÅÅF Y
request
ÅÅZ a
,
ÅÅa b
[
ÅÅc d
	FromQuery
ÅÅd m
]
ÅÅm n
Guid
ÅÅo s
	teacherId
ÅÅt }
)
ÅÅ} ~
{
ÇÇ 
var
ÉÉ 
result
ÉÉ 
=
ÉÉ 
await
ÉÉ 
	_mediator
ÉÉ $
.
ÉÉ$ %
Send
ÉÉ% )
(
ÉÉ) *
new
ÉÉ* -*
CreateClassroomLessonCommand
ÉÉ. J
(
ÉÉJ K
id
ÉÉK M
,
ÉÉM N
	teacherId
ÉÉO X
,
ÉÉX Y
request
ÉÉZ a
.
ÉÉa b
Title
ÉÉb g
,
ÉÉg h
request
ÉÉi p
.
ÉÉp q
Content
ÉÉq x
,
ÉÉx y
requestÉÉz Å
.ÉÉÅ Ç

OrderIndexÉÉÇ å
,ÉÉå ç
requestÉÉé ï
.ÉÉï ñ

DifficultyÉÉñ †
)ÉÉ† °
)ÉÉ° ¢
;ÉÉ¢ £
return
ÑÑ 
result
ÑÑ 
.
ÑÑ 
	IsSuccess
ÑÑ 
?
ÑÑ  !

StatusCode
ÑÑ" ,
(
ÑÑ, -
result
ÑÑ- 3
.
ÑÑ3 4

StatusCode
ÑÑ4 >
,
ÑÑ> ?
result
ÑÑ@ F
.
ÑÑF G
Data
ÑÑG K
)
ÑÑK L
:
ÑÑM N

BadRequest
ÑÑO Y
(
ÑÑY Z
result
ÑÑZ `
.
ÑÑ` a
Error
ÑÑa f
)
ÑÑf g
;
ÑÑg h
}
ÖÖ 
[
áá 
HttpGet
áá 
(
áá 
$str
áá &
)
áá& '
]
áá' (
public
àà 

async
àà 
Task
àà 
<
àà 
IActionResult
àà #
>
àà# $
GetLessonById
àà% 2
(
àà2 3
Guid
àà3 7
id
àà8 :
,
àà: ;
Guid
àà< @
lessonId
ààA I
,
ààI J
[
ààK L
	FromQuery
ààL U
]
ààU V
Guid
ààW [
userId
àà\ b
)
ààb c
{
ââ 
var
ää 
result
ää 
=
ää 
await
ää 
	_mediator
ää $
.
ää$ %
Send
ää% )
(
ää) *
new
ää* -)
GetClassroomLessonByIdQuery
ää. I
(
ääI J
id
ääJ L
,
ääL M
lessonId
ääN V
,
ääV W
userId
ääX ^
)
ää^ _
)
ää_ `
;
ää` a
return
ãã 
result
ãã 
.
ãã 
	IsSuccess
ãã 
?
ãã  !
Ok
ãã" $
(
ãã$ %
result
ãã% +
.
ãã+ ,
Data
ãã, 0
)
ãã0 1
:
ãã2 3

BadRequest
ãã4 >
(
ãã> ?
result
ãã? E
.
ããE F
Error
ããF K
)
ããK L
;
ããL M
}
åå 
[
éé 
HttpPut
éé 
(
éé 
$str
éé &
)
éé& '
]
éé' (
public
èè 

async
èè 
Task
èè 
<
èè 
IActionResult
èè #
>
èè# $
UpdateLesson
èè% 1
(
èè1 2
Guid
èè2 6
id
èè7 9
,
èè9 :
Guid
èè; ?
lessonId
èè@ H
,
èèH I
[
êê 	
FromBody
êê	 
]
êê *
UpdateClassroomLessonRequest
êê /
request
êê0 7
,
êê7 8
[
êê9 :
	FromQuery
êê: C
]
êêC D
Guid
êêE I
	teacherId
êêJ S
)
êêS T
{
ëë 
var
íí 
cmd
íí 
=
íí 
new
íí *
UpdateClassroomLessonCommand
íí 2
(
íí2 3
id
íí3 5
,
íí5 6
lessonId
íí7 ?
,
íí? @
	teacherId
ííA J
,
ííJ K
request
ìì 
.
ìì 
Title
ìì 
,
ìì 
request
ìì "
.
ìì" #
Content
ìì# *
,
ìì* +
request
ìì, 3
.
ìì3 4

Difficulty
ìì4 >
)
ìì> ?
;
ìì? @
var
îî 
result
îî 
=
îî 
await
îî 
	_mediator
îî $
.
îî$ %
Send
îî% )
(
îî) *
cmd
îî* -
)
îî- .
;
îî. /
return
ïï 
result
ïï 
.
ïï 
	IsSuccess
ïï 
?
ïï  !
Ok
ïï" $
(
ïï$ %
result
ïï% +
.
ïï+ ,
Data
ïï, 0
)
ïï0 1
:
ïï2 3

BadRequest
ïï4 >
(
ïï> ?
result
ïï? E
.
ïïE F
Error
ïïF K
)
ïïK L
;
ïïL M
}
ññ 
[
òò 

HttpDelete
òò 
(
òò 
$str
òò )
)
òò) *
]
òò* +
public
ôô 

async
ôô 
Task
ôô 
<
ôô 
IActionResult
ôô #
>
ôô# $
DeleteLesson
ôô% 1
(
ôô1 2
Guid
ôô2 6
id
ôô7 9
,
ôô9 :
Guid
ôô; ?
lessonId
ôô@ H
,
ôôH I
[
ôôJ K
	FromQuery
ôôK T
]
ôôT U
Guid
ôôV Z
	teacherId
ôô[ d
)
ôôd e
{
öö 
var
õõ 
result
õõ 
=
õõ 
await
õõ 
	_mediator
õõ $
.
õõ$ %
Send
õõ% )
(
õõ) *
new
õõ* -*
DeleteClassroomLessonCommand
õõ. J
(
õõJ K
id
õõK M
,
õõM N
lessonId
õõO W
,
õõW X
	teacherId
õõY b
)
õõb c
)
õõc d
;
õõd e
return
úú 
result
úú 
.
úú 
	IsSuccess
úú 
?
úú  !
Ok
úú" $
(
úú$ %
result
úú% +
.
úú+ ,
Data
úú, 0
)
úú0 1
:
úú2 3

BadRequest
úú4 >
(
úú> ?
result
úú? E
.
úúE F
Error
úúF K
)
úúK L
;
úúL M
}
ùù 
[
°° 
HttpPost
°° 
(
°° 
$str
°° /
)
°°/ 0
]
°°0 1
public
¢¢ 

async
¢¢ 
Task
¢¢ 
<
¢¢ 
IActionResult
¢¢ #
>
¢¢# $

CreateQuiz
¢¢% /
(
¢¢/ 0
Guid
¢¢0 4
id
¢¢5 7
,
¢¢7 8
Guid
¢¢9 =
lessonId
¢¢> F
,
¢¢F G
[
¢¢H I
FromBody
¢¢I Q
]
¢¢Q R
CreateQuizRequest
¢¢S d
request
¢¢e l
,
¢¢l m
[
¢¢n o
	FromQuery
¢¢o x
]
¢¢x y
Guid
¢¢z ~
	teacherId¢¢ à
)¢¢à â
{
££ 
var
§§ 
result
§§ 
=
§§ 
await
§§ 
	_mediator
§§ $
.
§§$ %
Send
§§% )
(
§§) *
new
§§* -(
CreateClassroomQuizCommand
§§. H
(
§§H I
id
§§I K
,
§§K L
lessonId
§§M U
,
§§U V
	teacherId
§§W `
,
§§` a
request
§§b i
.
§§i j
Title
§§j o
,
§§o p
request
§§q x
.
§§x y

Difficulty§§y É
,§§É Ñ
request§§Ö å
.§§å ç 
TimeLimitMinutes§§ç ù
)§§ù û
)§§û ü
;§§ü †
return
•• 
result
•• 
.
•• 
	IsSuccess
•• 
?
••  !

StatusCode
••" ,
(
••, -
result
••- 3
.
••3 4

StatusCode
••4 >
,
••> ?
result
••@ F
.
••F G
Data
••G K
)
••K L
:
••M N

BadRequest
••O Y
(
••Y Z
result
••Z `
.
••` a
Error
••a f
)
••f g
;
••g h
}
¶¶ 
[
®® 
HttpPost
®® 
(
®® 
$str
®® B
)
®®B C
]
®®C D
public
©© 

async
©© 
Task
©© 
<
©© 
IActionResult
©© #
>
©©# $
AddQuestion
©©% 0
(
©©0 1
Guid
©©1 5
id
©©6 8
,
©©8 9
Guid
©©: >
quizId
©©? E
,
©©E F
[
©©G H
FromBody
©©H P
]
©©P Q 
AddQuestionRequest
©©R d
request
©©e l
,
©©l m
[
©©n o
	FromQuery
©©o x
]
©©x y
Guid
©©z ~
	teacherId©© à
)©©à â
{
™™ 
var
´´ 
result
´´ 
=
´´ 
await
´´ 
	_mediator
´´ $
.
´´$ %
Send
´´% )
(
´´) *
new
´´* -)
AddClassroomQuestionCommand
´´. I
(
´´I J
id
´´J L
,
´´L M
quizId
´´N T
,
´´T U
	teacherId
´´V _
,
´´_ `
request
´´a h
.
´´h i
Text
´´i m
,
´´m n
request
´´o v
.
´´v w
CorrectAnswer´´w Ñ
,´´Ñ Ö
request´´Ü ç
.´´ç é
Options´´é ï
,´´ï ñ
request´´ó û
.´´û ü
Points´´ü •
,´´• ¶
request´´ß Æ
.´´Æ Ø
Explanation´´Ø ∫
)´´∫ ª
)´´ª º
;´´º Ω
return
¨¨ 
result
¨¨ 
.
¨¨ 
	IsSuccess
¨¨ 
?
¨¨  !

StatusCode
¨¨" ,
(
¨¨, -
result
¨¨- 3
.
¨¨3 4

StatusCode
¨¨4 >
,
¨¨> ?
result
¨¨@ F
.
¨¨F G
Data
¨¨G K
)
¨¨K L
:
¨¨M N

BadRequest
¨¨O Y
(
¨¨Y Z
result
¨¨Z `
.
¨¨` a
Error
¨¨a f
)
¨¨f g
;
¨¨g h
}
≠≠ 
[
ØØ 
HttpGet
ØØ 
(
ØØ 
$str
ØØ .
)
ØØ. /
]
ØØ/ 0
public
∞∞ 

async
∞∞ 
Task
∞∞ 
<
∞∞ 
IActionResult
∞∞ #
>
∞∞# $ 
GetQuizzesByLesson
∞∞% 7
(
∞∞7 8
Guid
∞∞8 <
id
∞∞= ?
,
∞∞? @
Guid
∞∞A E
lessonId
∞∞F N
,
∞∞N O
[
∞∞P Q
	FromQuery
∞∞Q Z
]
∞∞Z [
Guid
∞∞\ `
userId
∞∞a g
)
∞∞g h
{
±± 
var
≤≤ 
result
≤≤ 
=
≤≤ 
await
≤≤ 
	_mediator
≤≤ $
.
≤≤$ %
Send
≤≤% )
(
≤≤) *
new
≤≤* -.
 GetClassroomQuizzesByLessonQuery
≤≤. N
(
≤≤N O
id
≤≤O Q
,
≤≤Q R
lessonId
≤≤S [
,
≤≤[ \
userId
≤≤] c
)
≤≤c d
)
≤≤d e
;
≤≤e f
return
≥≥ 
result
≥≥ 
.
≥≥ 
	IsSuccess
≥≥ 
?
≥≥  !
Ok
≥≥" $
(
≥≥$ %
result
≥≥% +
.
≥≥+ ,
Data
≥≥, 0
)
≥≥0 1
:
≥≥2 3

BadRequest
≥≥4 >
(
≥≥> ?
result
≥≥? E
.
≥≥E F
Error
≥≥F K
)
≥≥K L
;
≥≥L M
}
¥¥ 
[
∂∂ 
HttpGet
∂∂ 
(
∂∂ 
$str
∂∂ $
)
∂∂$ %
]
∂∂% &
public
∑∑ 

async
∑∑ 
Task
∑∑ 
<
∑∑ 
IActionResult
∑∑ #
>
∑∑# $
GetQuizById
∑∑% 0
(
∑∑0 1
Guid
∑∑1 5
id
∑∑6 8
,
∑∑8 9
Guid
∑∑: >
quizId
∑∑? E
,
∑∑E F
[
∑∑G H
	FromQuery
∑∑H Q
]
∑∑Q R
Guid
∑∑S W
userId
∑∑X ^
)
∑∑^ _
{
∏∏ 
var
ππ 
result
ππ 
=
ππ 
await
ππ 
	_mediator
ππ $
.
ππ$ %
Send
ππ% )
(
ππ) *
new
ππ* -'
GetClassroomQuizByIdQuery
ππ. G
(
ππG H
id
ππH J
,
ππJ K
quizId
ππL R
,
ππR S
userId
ππT Z
)
ππZ [
)
ππ[ \
;
ππ\ ]
return
∫∫ 
result
∫∫ 
.
∫∫ 
	IsSuccess
∫∫ 
?
∫∫  !
Ok
∫∫" $
(
∫∫$ %
result
∫∫% +
.
∫∫+ ,
Data
∫∫, 0
)
∫∫0 1
:
∫∫2 3

BadRequest
∫∫4 >
(
∫∫> ?
result
∫∫? E
.
∫∫E F
Error
∫∫F K
)
∫∫K L
;
∫∫L M
}
ªª 
[
ΩΩ 
HttpPut
ΩΩ 
(
ΩΩ 
$str
ΩΩ $
)
ΩΩ$ %
]
ΩΩ% &
public
ææ 

async
ææ 
Task
ææ 
<
ææ 
IActionResult
ææ #
>
ææ# $

UpdateQuiz
ææ% /
(
ææ/ 0
Guid
ææ0 4
id
ææ5 7
,
ææ7 8
Guid
ææ9 =
quizId
ææ> D
,
ææD E
[
øø 	
FromBody
øø	 
]
øø (
UpdateClassroomQuizRequest
øø -
request
øø. 5
,
øø5 6
[
øø7 8
	FromQuery
øø8 A
]
øøA B
Guid
øøC G
	teacherId
øøH Q
)
øøQ R
{
¿¿ 
var
¡¡ 
cmd
¡¡ 
=
¡¡ 
new
¡¡ (
UpdateClassroomQuizCommand
¡¡ 0
(
¡¡0 1
id
¡¡1 3
,
¡¡3 4
quizId
¡¡5 ;
,
¡¡; <
	teacherId
¡¡= F
,
¡¡F G
request
¬¬ 
.
¬¬ 
Title
¬¬ 
,
¬¬ 
request
¬¬ "
.
¬¬" #

Difficulty
¬¬# -
,
¬¬- .
request
¬¬/ 6
.
¬¬6 7
TimeLimitMinutes
¬¬7 G
)
¬¬G H
;
¬¬H I
var
√√ 
result
√√ 
=
√√ 
await
√√ 
	_mediator
√√ $
.
√√$ %
Send
√√% )
(
√√) *
cmd
√√* -
)
√√- .
;
√√. /
return
ƒƒ 
result
ƒƒ 
.
ƒƒ 
	IsSuccess
ƒƒ 
?
ƒƒ  !
Ok
ƒƒ" $
(
ƒƒ$ %
result
ƒƒ% +
.
ƒƒ+ ,
Data
ƒƒ, 0
)
ƒƒ0 1
:
ƒƒ2 3

BadRequest
ƒƒ4 >
(
ƒƒ> ?
result
ƒƒ? E
.
ƒƒE F
Error
ƒƒF K
)
ƒƒK L
;
ƒƒL M
}
≈≈ 
[
«« 

HttpDelete
«« 
(
«« 
$str
«« '
)
««' (
]
««( )
public
»» 

async
»» 
Task
»» 
<
»» 
IActionResult
»» #
>
»»# $

DeleteQuiz
»»% /
(
»»/ 0
Guid
»»0 4
id
»»5 7
,
»»7 8
Guid
»»9 =
quizId
»»> D
,
»»D E
[
»»F G
	FromQuery
»»G P
]
»»P Q
Guid
»»R V
	teacherId
»»W `
)
»»` a
{
…… 
var
   
result
   
=
   
await
   
	_mediator
   $
.
  $ %
Send
  % )
(
  ) *
new
  * -(
DeleteClassroomQuizCommand
  . H
(
  H I
id
  I K
,
  K L
quizId
  M S
,
  S T
	teacherId
  U ^
)
  ^ _
)
  _ `
;
  ` a
return
ÀÀ 
result
ÀÀ 
.
ÀÀ 
	IsSuccess
ÀÀ 
?
ÀÀ  !
Ok
ÀÀ" $
(
ÀÀ$ %
result
ÀÀ% +
.
ÀÀ+ ,
Data
ÀÀ, 0
)
ÀÀ0 1
:
ÀÀ2 3

BadRequest
ÀÀ4 >
(
ÀÀ> ?
result
ÀÀ? E
.
ÀÀE F
Error
ÀÀF K
)
ÀÀK L
;
ÀÀL M
}
ÃÃ 
[
ŒŒ 

HttpDelete
ŒŒ 
(
ŒŒ 
$str
ŒŒ >
)
ŒŒ> ?
]
ŒŒ? @
public
œœ 

async
œœ 
Task
œœ 
<
œœ 
IActionResult
œœ #
>
œœ# $
DeleteQuestion
œœ% 3
(
œœ3 4
Guid
œœ4 8
id
œœ9 ;
,
œœ; <
Guid
œœ= A
quizId
œœB H
,
œœH I
Guid
œœJ N

questionId
œœO Y
,
œœY Z
[
œœ[ \
	FromQuery
œœ\ e
]
œœe f
Guid
œœg k
	teacherId
œœl u
)
œœu v
{
–– 
var
—— 
result
—— 
=
—— 
await
—— 
	_mediator
—— $
.
——$ %
Send
——% )
(
——) *
new
——* -,
DeleteClassroomQuestionCommand
——. L
(
——L M
id
——M O
,
——O P
quizId
——Q W
,
——W X

questionId
——Y c
,
——c d
	teacherId
——e n
)
——n o
)
——o p
;
——p q
return
““ 
result
““ 
.
““ 
	IsSuccess
““ 
?
““  !
Ok
““" $
(
““$ %
result
““% +
.
““+ ,
Data
““, 0
)
““0 1
:
““2 3

BadRequest
““4 >
(
““> ?
result
““? E
.
““E F
Error
““F K
)
““K L
;
““L M
}
”” 
[
÷÷ 
HttpPost
÷÷ 
(
÷÷ 
$str
÷÷ ?
)
÷÷? @
]
÷÷@ A
public
◊◊ 

async
◊◊ 
Task
◊◊ 
<
◊◊ 
IActionResult
◊◊ #
>
◊◊# $

SubmitQuiz
◊◊% /
(
◊◊/ 0
Guid
◊◊0 4
id
◊◊5 7
,
◊◊7 8
Guid
◊◊9 =
quizId
◊◊> D
,
◊◊D E
[
◊◊F G
FromBody
◊◊G O
]
◊◊O P
SubmitQuizRequest
◊◊Q b
request
◊◊c j
,
◊◊j k
[
◊◊l m
	FromQuery
◊◊m v
]
◊◊v w
Guid
◊◊x |
	studentId◊◊} Ü
)◊◊Ü á
{
ÿÿ 
var
ŸŸ 
result
ŸŸ 
=
ŸŸ 
await
ŸŸ 
	_mediator
ŸŸ $
.
ŸŸ$ %
Send
ŸŸ% )
(
ŸŸ) *
new
ŸŸ* -(
SubmitClassroomQuizCommand
ŸŸ. H
(
ŸŸH I
id
ŸŸI K
,
ŸŸK L
quizId
ŸŸM S
,
ŸŸS T
	studentId
ŸŸU ^
,
ŸŸ^ _
request
ŸŸ` g
.
ŸŸg h
Answers
ŸŸh o
)
ŸŸo p
)
ŸŸp q
;
ŸŸq r
return
⁄⁄ 
result
⁄⁄ 
.
⁄⁄ 
	IsSuccess
⁄⁄ 
?
⁄⁄  !
Ok
⁄⁄" $
(
⁄⁄$ %
result
⁄⁄% +
.
⁄⁄+ ,
Data
⁄⁄, 0
)
⁄⁄0 1
:
⁄⁄2 3

BadRequest
⁄⁄4 >
(
⁄⁄> ?
result
⁄⁄? E
.
⁄⁄E F
Error
⁄⁄F K
)
⁄⁄K L
;
⁄⁄L M
}
€€ 
[
ﬁﬁ 
HttpGet
ﬁﬁ 
(
ﬁﬁ 
$str
ﬁﬁ 
)
ﬁﬁ 
]
ﬁﬁ 
public
ﬂﬂ 

async
ﬂﬂ 
Task
ﬂﬂ 
<
ﬂﬂ 
IActionResult
ﬂﬂ #
>
ﬂﬂ# $
GetProgress
ﬂﬂ% 0
(
ﬂﬂ0 1
Guid
ﬂﬂ1 5
id
ﬂﬂ6 8
,
ﬂﬂ8 9
[
ﬂﬂ: ;
	FromQuery
ﬂﬂ; D
]
ﬂﬂD E
Guid
ﬂﬂF J
userId
ﬂﬂK Q
)
ﬂﬂQ R
{
‡‡ 
var
·· 
result
·· 
=
·· 
await
·· 
	_mediator
·· $
.
··$ %
Send
··% )
(
··) *
new
··* -'
GetClassroomProgressQuery
··. G
(
··G H
id
··H J
,
··J K
userId
··L R
)
··R S
)
··S T
;
··T U
return
‚‚ 
result
‚‚ 
.
‚‚ 
	IsSuccess
‚‚ 
?
‚‚  !
Ok
‚‚" $
(
‚‚$ %
result
‚‚% +
.
‚‚+ ,
Data
‚‚, 0
)
‚‚0 1
:
‚‚2 3

BadRequest
‚‚4 >
(
‚‚> ?
result
‚‚? E
.
‚‚E F
Error
‚‚F K
)
‚‚K L
;
‚‚L M
}
„„ 
[
ÊÊ 
HttpGet
ÊÊ 
(
ÊÊ 
$str
ÊÊ 
)
ÊÊ 
]
ÊÊ 
public
ÁÁ 

async
ÁÁ 
Task
ÁÁ 
<
ÁÁ 
IActionResult
ÁÁ #
>
ÁÁ# $
	GetGrades
ÁÁ% .
(
ÁÁ. /
Guid
ÁÁ/ 3
id
ÁÁ4 6
,
ÁÁ6 7
[
ÁÁ8 9
	FromQuery
ÁÁ9 B
]
ÁÁB C
Guid
ÁÁD H
userId
ÁÁI O
,
ÁÁO P
[
ÁÁQ R
	FromQuery
ÁÁR [
]
ÁÁ[ \
bool
ÁÁ] a
	isTeacher
ÁÁb k
=
ÁÁl m
false
ÁÁn s
)
ÁÁs t
{
ËË 
var
ÈÈ 
result
ÈÈ 
=
ÈÈ 
await
ÈÈ 
	_mediator
ÈÈ $
.
ÈÈ$ %
Send
ÈÈ% )
(
ÈÈ) *
new
ÈÈ* -%
GetClassroomGradesQuery
ÈÈ. E
(
ÈÈE F
id
ÈÈF H
,
ÈÈH I
userId
ÈÈJ P
,
ÈÈP Q
	isTeacher
ÈÈR [
)
ÈÈ[ \
)
ÈÈ\ ]
;
ÈÈ] ^
return
ÍÍ 
result
ÍÍ 
.
ÍÍ 
	IsSuccess
ÍÍ 
?
ÍÍ  !
Ok
ÍÍ" $
(
ÍÍ$ %
result
ÍÍ% +
.
ÍÍ+ ,
Data
ÍÍ, 0
)
ÍÍ0 1
:
ÍÍ2 3

BadRequest
ÍÍ4 >
(
ÍÍ> ?
result
ÍÍ? E
.
ÍÍE F
Error
ÍÍF K
)
ÍÍK L
;
ÍÍL M
}
ÎÎ 
[
ÌÌ 
HttpPost
ÌÌ 
(
ÌÌ 
$str
ÌÌ 
)
ÌÌ 
]
ÌÌ 
public
ÓÓ 

async
ÓÓ 
Task
ÓÓ 
<
ÓÓ 
IActionResult
ÓÓ #
>
ÓÓ# $
AddGrade
ÓÓ% -
(
ÓÓ- .
Guid
ÓÓ. 2
id
ÓÓ3 5
,
ÓÓ5 6
[
ÓÓ7 8
FromBody
ÓÓ8 @
]
ÓÓ@ A
AddGradeRequest
ÓÓB Q
request
ÓÓR Y
,
ÓÓY Z
[
ÓÓ[ \
	FromQuery
ÓÓ\ e
]
ÓÓe f
Guid
ÓÓg k
	teacherId
ÓÓl u
)
ÓÓu v
{
ÔÔ 
var
 
result
 
=
 
await
 
	_mediator
 $
.
$ %
Send
% )
(
) *
new
* -
AddGradeCommand
. =
(
= >
id
> @
,
@ A
request
B I
.
I J
	StudentId
J S
,
S T
	teacherId
U ^
,
^ _
request
` g
.
g h
Value
h m
,
m n
request
o v
.
v w
Descriptionw Ç
??É Ö
stringÜ å
.å ç
Emptyç í
)í ì
)ì î
;î ï
return
ÒÒ 
result
ÒÒ 
.
ÒÒ 
	IsSuccess
ÒÒ 
?
ÒÒ  !

StatusCode
ÒÒ" ,
(
ÒÒ, -
result
ÒÒ- 3
.
ÒÒ3 4

StatusCode
ÒÒ4 >
,
ÒÒ> ?
result
ÒÒ@ F
.
ÒÒF G
Data
ÒÒG K
)
ÒÒK L
:
ÒÒM N

BadRequest
ÒÒO Y
(
ÒÒY Z
result
ÒÒZ `
.
ÒÒ` a
Error
ÒÒa f
)
ÒÒf g
;
ÒÒg h
}
ÚÚ 
[
ÙÙ 
HttpPut
ÙÙ 
(
ÙÙ 
$str
ÙÙ $
)
ÙÙ$ %
]
ÙÙ% &
public
ıı 

async
ıı 
Task
ıı 
<
ıı 
IActionResult
ıı #
>
ıı# $
UpdateGrade
ıı% 0
(
ıı0 1
Guid
ıı1 5
id
ıı6 8
,
ıı8 9
Guid
ıı: >
gradeId
ıı? F
,
ııF G
[
ııH I
FromBody
ııI Q
]
ııQ R 
UpdateGradeRequest
ııS e
request
ııf m
,
ıım n
[
ııo p
	FromQuery
ııp y
]
ııy z
Guid
ıı{ 
	teacherIdııÄ â
)ııâ ä
{
ˆˆ 
var
˜˜ 
result
˜˜ 
=
˜˜ 
await
˜˜ 
	_mediator
˜˜ $
.
˜˜$ %
Send
˜˜% )
(
˜˜) *
new
˜˜* - 
UpdateGradeCommand
˜˜. @
(
˜˜@ A
gradeId
˜˜A H
,
˜˜H I
	teacherId
˜˜J S
,
˜˜S T
request
˜˜U \
.
˜˜\ ]
Value
˜˜] b
,
˜˜b c
request
˜˜d k
.
˜˜k l
Description
˜˜l w
??
˜˜x z
string˜˜{ Å
.˜˜Å Ç
Empty˜˜Ç á
)˜˜á à
)˜˜à â
;˜˜â ä
return
¯¯ 
result
¯¯ 
.
¯¯ 
	IsSuccess
¯¯ 
?
¯¯  !
Ok
¯¯" $
(
¯¯$ %
result
¯¯% +
.
¯¯+ ,
Data
¯¯, 0
)
¯¯0 1
:
¯¯2 3

BadRequest
¯¯4 >
(
¯¯> ?
result
¯¯? E
.
¯¯E F
Error
¯¯F K
)
¯¯K L
;
¯¯L M
}
˘˘ 
[
˚˚ 

HttpDelete
˚˚ 
(
˚˚ 
$str
˚˚ '
)
˚˚' (
]
˚˚( )
public
¸¸ 

async
¸¸ 
Task
¸¸ 
<
¸¸ 
IActionResult
¸¸ #
>
¸¸# $
DeleteGrade
¸¸% 0
(
¸¸0 1
Guid
¸¸1 5
id
¸¸6 8
,
¸¸8 9
Guid
¸¸: >
gradeId
¸¸? F
,
¸¸F G
[
¸¸H I
	FromQuery
¸¸I R
]
¸¸R S
Guid
¸¸T X
	teacherId
¸¸Y b
)
¸¸b c
{
˝˝ 
var
˛˛ 
result
˛˛ 
=
˛˛ 
await
˛˛ 
	_mediator
˛˛ $
.
˛˛$ %
Send
˛˛% )
(
˛˛) *
new
˛˛* - 
DeleteGradeCommand
˛˛. @
(
˛˛@ A
gradeId
˛˛A H
,
˛˛H I
	teacherId
˛˛J S
)
˛˛S T
)
˛˛T U
;
˛˛U V
return
ˇˇ 
result
ˇˇ 
.
ˇˇ 
	IsSuccess
ˇˇ 
?
ˇˇ  !
Ok
ˇˇ" $
(
ˇˇ$ %
)
ˇˇ% &
:
ˇˇ' (

BadRequest
ˇˇ) 3
(
ˇˇ3 4
result
ˇˇ4 :
.
ˇˇ: ;
Error
ˇˇ; @
)
ˇˇ@ A
;
ˇˇA B
}
ÄÄ 
public
ÇÇ 

record
ÇÇ *
UpdateClassroomLessonRequest
ÇÇ .
(
ÇÇ. /
string
ÉÉ 

Title
ÉÉ 
,
ÉÉ 
string
ÑÑ 

Content
ÑÑ 
,
ÑÑ 
AiTutor
ÖÖ 
.
ÖÖ 
Domain
ÖÖ 
.
ÖÖ 
Enums
ÖÖ 
.
ÖÖ 
DifficultyLevel
ÖÖ (

Difficulty
ÖÖ) 3
)ÜÜ 
;
ÜÜ 
publicàà 
record
àà (
UpdateClassroomQuizRequest
àà (
(
àà( )
string
ââ 

Title
ââ 
,
ââ 
AiTutor
ää 
.
ää 
Domain
ää 
.
ää 
Enums
ää 
.
ää 
DifficultyLevel
ää (

Difficulty
ää) 3
,
ää3 4
int
ãã 
TimeLimitMinutes
ãã 
)åå 
;
åå 
}èè ˘
SD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.API\Controllers\LessonsController.cs
	namespace 	
AiTutor
 
. 
API 
. 
Controllers !
;! "
[		 
	Authorize		 

]		
 
public

 
class

 
LessonsController

 
:

  
BaseController

! /
{ 
[ 
HttpGet 
( 
$str 
) 
] 
public 

async 
Task 
< 
IActionResult #
># $
GetById% ,
(, -
Guid- 1
id2 4
)4 5
{ 
var 
result 
= 
await 
Mediator #
.# $
Send$ (
(( )
new) ,
GetLessonByIdQuery- ?
(? @
id@ B
)B C
)C D
;D E
if 

( 
! 
result 
. 
	IsSuccess 
) 
return 

StatusCode 
( 
result $
.$ %

StatusCode% /
,/ 0
new1 4
{5 6
message7 >
=? @
resultA G
.G H
ErrorH M
}N O
)O P
;P Q
return 
Ok 
( 
result 
. 
Data 
) 
; 
} 
[ 
HttpGet 
( 
$str '
)' (
]( )
public 

async 
Task 
< 
IActionResult #
># $
GetBySubjectId% 3
(3 4
Guid4 8
	subjectId9 B
)B C
{ 
var 
result 
= 
await 
Mediator #
.# $
Send$ (
(( )
new) ,$
GetLessonsBySubjectQuery- E
(E F
	subjectIdF O
)O P
)P Q
;Q R
return 
Ok 
( 
result 
. 
Data 
) 
; 
} 
[ 
HttpPost 
] 
[ 
	Authorize 
( 
Roles 
= 
$str &
)& '
]' (
public 

async 
Task 
< 
IActionResult #
># $
Create% +
(+ ,
[, -
FromBody- 5
]5 6
CreateLessonCommand7 J
commandK R
)R S
{ 
var   
result   
=   
await   
Mediator   #
.  # $
Send  $ (
(  ( )
command  ) 0
)  0 1
;  1 2
if!! 

(!! 
!!! 
result!! 
.!! 
	IsSuccess!! 
)!! 
return"" 

StatusCode"" 
("" 
result"" $
.""$ %

StatusCode""% /
,""/ 0
new""1 4
{""5 6
message""7 >
=""? @
result""A G
.""G H
Error""H M
}""N O
)""O P
;""P Q
return$$ 

StatusCode$$ 
($$ 
$num$$ 
,$$ 
result$$ %
.$$% &
Data$$& *
)$$* +
;$$+ ,
}%% 
}&& á
TD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.API\Controllers\ProgressController.cs
	namespace 	
AiTutor
 
. 
API 
. 
Controllers !
;! "
[ 
	Authorize 

]
 
public		 
class		 
ProgressController		 
:		  !
BaseController		" 0
{

 
[ 
HttpGet 
( 
$str 
) 
] 
public 

async 
Task 
< 
IActionResult #
># $
GetStudentProgress% 7
(7 8
Guid8 <
userId= C
)C D
{ 
var 
result 
= 
await 
Mediator #
.# $
Send$ (
(( )
new) ,#
GetStudentProgressQuery- D
(D E
userIdE K
)K L
)L M
;M N
return 
Ok 
( 
result 
. 
Data 
) 
; 
} 
[ 
HttpPost 
( 
$str 
) 
] 
public 

async 
Task 
< 
IActionResult #
># $
CompleteLesson% 3
(3 4
[4 5
FromBody5 =
]= >!
CompleteLessonCommand? T
commandU \
)\ ]
{ 
var 
result 
= 
await 
Mediator #
.# $
Send$ (
(( )
command) 0
)0 1
;1 2
if 

( 
! 
result 
. 
	IsSuccess 
) 
return 

StatusCode 
( 
result $
.$ %

StatusCode% /
,/ 0
new1 4
{5 6
message7 >
=? @
resultA G
.G H
ErrorH M
}N O
)O P
;P Q
return 
Ok 
( 
result 
. 
Data 
) 
; 
} 
} Â/
SD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.API\Controllers\QuizzesController.cs
	namespace		 	
AiTutor		
 
.		 
API		 
.		 
Controllers		 !
;		! "
[ 
	Authorize 

]
 
public 
class 
QuizzesController 
:  
BaseController! /
{ 
[ 
HttpGet 
( 
$str 
) 
] 
public 

async 
Task 
< 
IActionResult #
># $
GetById% ,
(, -
Guid- 1
id2 4
)4 5
{ 
var 
result 
= 
await 
Mediator #
.# $
Send$ (
(( )
new) ,
GetQuizByIdQuery- =
(= >
id> @
)@ A
)A B
;B C
return 
Ok 
( 
result 
. 
Data 
) 
; 
} 
[ 
HttpGet 
( 
$str %
)% &
]& '
public 

async 
Task 
< 
IActionResult #
># $
GetByLesson% 0
(0 1
Guid1 5
lessonId6 >
)> ?
{ 
var 
result 
= 
await 
Mediator #
.# $
Send$ (
(( )
new) ,#
GetQuizzesByLessonQuery- D
(D E
lessonIdE M
)M N
)N O
;O P
return 
Ok 
( 
result 
. 
Data 
) 
; 
} 
[ 
HttpPost 
] 
[ 
	Authorize 
( 
Roles 
= 
$str &
)& '
]' (
public 

async 
Task 
< 
IActionResult #
># $
Create% +
(+ ,
[, -
FromBody- 5
]5 6
CreateQuizCommand7 H
commandI P
)P Q
{ 
var   
result   
=   
await   
Mediator   #
.  # $
Send  $ (
(  ( )
command  ) 0
)  0 1
;  1 2
if!! 

(!! 
!!! 
result!! 
.!! 
	IsSuccess!! 
)!! 
return"" 

StatusCode"" 
("" 
result"" $
.""$ %

StatusCode""% /
,""/ 0
new""1 4
{""5 6
message""7 >
=""? @
result""A G
.""G H
Error""H M
}""N O
)""O P
;""P Q
return## 

StatusCode## 
(## 
$num## 
,## 
result## %
.##% &
Data##& *
)##* +
;##+ ,
}$$ 
[&& 
HttpPost&& 
(&& 
$str&& #
)&&# $
]&&$ %
['' 
	Authorize'' 
('' 
Roles'' 
='' 
$str'' &
)''& '
]''' (
public(( 

async(( 
Task(( 
<(( 
IActionResult(( #
>((# $
AddQuestion((% 0
(((0 1
Guid((1 5
id((6 8
,((8 9
[)) 	
FromBody))	 
])) !
CreateQuestionCommand)) (
command))) 0
)))0 1
{** 
var++ 
commandWithId++ 
=++ 
command++ #
with++$ (
{++) *
QuizId+++ 1
=++2 3
id++4 6
}++7 8
;++8 9
var,, 
result,, 
=,, 
await,, 
Mediator,, #
.,,# $
Send,,$ (
(,,( )
commandWithId,,) 6
),,6 7
;,,7 8
if-- 

(-- 
!-- 
result-- 
.-- 
	IsSuccess-- 
)-- 
return.. 

StatusCode.. 
(.. 
result.. $
...$ %

StatusCode..% /
,../ 0
new..1 4
{..5 6
message..7 >
=..? @
result..A G
...G H
Error..H M
}..N O
)..O P
;..P Q
return// 

StatusCode// 
(// 
$num// 
,// 
result// %
.//% &
Data//& *
)//* +
;//+ ,
}00 
[22 
HttpPost22 
(22 
$str22  
)22  !
]22! "
public33 

async33 
Task33 
<33 
IActionResult33 #
>33# $
Submit33% +
(33+ ,
Guid33, 0
id331 3
,333 4
[44 	
FromBody44	 
]44 
SubmitQuizCommand44 $
command44% ,
)44, -
{55 
var66 
commandWithId66 
=66 
command66 #
with66$ (
{66) *
QuizId66+ 1
=662 3
id664 6
}667 8
;668 9
var77 
result77 
=77 
await77 
Mediator77 #
.77# $
Send77$ (
(77( )
commandWithId77) 6
)776 7
;777 8
if88 

(88 
!88 
result88 
.88 
	IsSuccess88 
)88 
return99 

StatusCode99 
(99 
result99 $
.99$ %

StatusCode99% /
,99/ 0
new991 4
{995 6
message997 >
=99? @
result99A G
.99G H
Error99H M
}99N O
)99O P
;99P Q
return:: 
Ok:: 
(:: 
result:: 
.:: 
Data:: 
):: 
;:: 
};; 
}<< Ñ
TD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.API\Controllers\SubjectsController.cs
	namespace 	
AiTutor
 
. 
API 
. 
Controllers !
;! "
[ 
	Authorize 

]
 
public		 
class		 
SubjectsController		 
:		  !
BaseController		" 0
{

 
[ 
HttpGet 
] 
public 

async 
Task 
< 
IActionResult #
># $
GetAll% +
(+ ,
), -
{ 
var 
result 
= 
await 
Mediator #
.# $
Send$ (
(( )
new) ,
GetAllSubjectsQuery- @
(@ A
)A B
)B C
;C D
return 
Ok 
( 
result 
. 
Data 
) 
; 
} 
[ 
HttpPost 
] 
[ 
	Authorize 
( 
Roles 
= 
$str &
)& '
]' (
public 

async 
Task 
< 
IActionResult #
># $
Create% +
(+ ,
[, -
FromBody- 5
]5 6 
CreateSubjectCommand7 K
commandL S
)S T
{ 
var 
result 
= 
await 
Mediator #
.# $
Send$ (
(( )
command) 0
)0 1
;1 2
if 

( 
! 
result 
. 
	IsSuccess 
) 
return 

StatusCode 
( 
result $
.$ %

StatusCode% /
,/ 0
new1 4
{5 6
message7 >
=? @
resultA G
.G H
ErrorH M
}N O
)O P
;P Q
return 

StatusCode 
( 
$num 
, 
result %
.% &
Data& *
)* +
;+ ,
} 
} ⁄M
YD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.API\Controllers\SubscriptionsController.cs
	namespace 	
AiTutor
 
. 
API 
. 
Controllers !
;! "
[ 
ApiController 
] 
[ 
Route 
( 
$str 
) 
] 
[ 
	Authorize 

]
 
public 
class #
SubscriptionsController $
:% &
ControllerBase' 5
{ 
private 
readonly 
	IMediator 
	_mediator (
;( )
public 
#
SubscriptionsController "
(" #
	IMediator# ,
mediator- 5
)5 6
=>7 9
	_mediator: C
=D E
mediatorF N
;N O
public 

record %
CreateSubscriptionRequest +
(+ ,
Guid 
UserId 
, 
SubscriptionType 
Type 
, 
decimal 
Price 
, 
DateTime 
	StartDate 
, 
DateTime 
EndDate 
) 
; 
public 

record (
CreateCheckoutSessionRequest .
(. /
Guid 
UserId 
, 
SubscriptionType   
SubscriptionType   )
)!! 
;!! 
[## 
HttpGet## 
]## 
[$$ 
	Authorize$$ 
($$ 
Roles$$ 
=$$ 
$str$$ 
)$$ 
]$$  
public%% 

async%% 
Task%% 
<%% 
IActionResult%% #
>%%# $
GetAll%%% +
(%%+ ,
[%%, -
	FromQuery%%- 6
]%%6 7
int%%8 ;
page%%< @
=%%A B
$num%%C D
,%%D E
[%%F G
	FromQuery%%G P
]%%P Q
int%%R U
pageSize%%V ^
=%%_ `
$num%%a c
)%%c d
{&& 
var'' 
result'' 
='' 
await'' 
	_mediator'' $
.''$ %
Send''% )
('') *
new''* -$
GetAllSubscriptionsQuery''. F
(''F G
page''G K
,''K L
pageSize''M U
)''U V
)''V W
;''W X
return(( 
result(( 
.(( 
	IsSuccess(( 
?((  !
Ok((" $
((($ %
result((% +
.((+ ,
Data((, 0
)((0 1
:((2 3

BadRequest((4 >
(((> ?
result((? E
.((E F
Error((F K
)((K L
;((L M
})) 
[++ 
HttpGet++ 
(++ 
$str++ 
)++ 
]++ 
public,, 

async,, 
Task,, 
<,, 
IActionResult,, #
>,,# $
	GetByUser,,% .
(,,. /
Guid,,/ 3
userId,,4 :
),,: ;
{-- 
var.. 
result.. 
=.. 
await.. 
	_mediator.. $
...$ %
Send..% )
(..) *
new..* -&
GetSubscriptionByUserQuery... H
(..H I
userId..I O
)..O P
)..P Q
;..Q R
return// 
result// 
.// 
	IsSuccess// 
?//  !
Ok//" $
(//$ %
result//% +
.//+ ,
Data//, 0
)//0 1
://2 3
NotFound//4 <
(//< =
result//= C
.//C D
Error//D I
)//I J
;//J K
}00 
[22 
HttpPost22 
]22 
public33 

async33 
Task33 
<33 
IActionResult33 #
>33# $
Create33% +
(33+ ,
[33, -
FromBody33- 5
]335 6%
CreateSubscriptionRequest337 P
request33Q X
)33X Y
{44 
var55 
command55 
=55 
new55 %
CreateSubscriptionCommand55 3
(553 4
request66 
.66 
UserId66 
,66 
request77 
.77 
Type77 
,77 
request88 
.88 
Price88 
,88 
request99 
.99 
	StartDate99 
,99 
request:: 
.:: 
EndDate:: 
);; 	
;;;	 

var<< 
result<< 
=<< 
await<< 
	_mediator<< $
.<<$ %
Send<<% )
(<<) *
command<<* 1
)<<1 2
;<<2 3
return== 
result== 
.== 
	IsSuccess== 
?>> 

StatusCode>> 
(>> 
result>> 
.>>  

StatusCode>>  *
,>>* +
result>>, 2
.>>2 3
Data>>3 7
)>>7 8
:?? 

BadRequest?? 
(?? 
result?? 
.??  
Error??  %
)??% &
;??& '
}@@ 
[BB 

HttpDeleteBB 
(BB 
$strBB 
)BB 
]BB 
publicCC 

asyncCC 
TaskCC 
<CC 
IActionResultCC #
>CC# $
CancelCC% +
(CC+ ,
GuidCC, 0
idCC1 3
,CC3 4
[CC5 6
	FromQueryCC6 ?
]CC? @
GuidCCA E
userIdCCF L
)CCL M
{DD 
varEE 
resultEE 
=EE 
awaitEE 
	_mediatorEE $
.EE$ %
SendEE% )
(EE) *
newEE* -%
CancelSubscriptionCommandEE. G
(EEG H
idEEH J
,EEJ K
userIdEEL R
)EER S
)EES T
;EET U
returnFF 
resultFF 
.FF 
	IsSuccessFF 
?FF  !
OkFF" $
(FF$ %
)FF% &
:FF' (

BadRequestFF) 3
(FF3 4
resultFF4 :
.FF: ;
ErrorFF; @
)FF@ A
;FFA B
}GG 
[MM 
HttpPostMM 
(MM 
$strMM  
)MM  !
]MM! "
publicNN 

asyncNN 
TaskNN 
<NN 
IActionResultNN #
>NN# $!
CreateCheckoutSessionNN% :
(NN: ;
[OO 	
FromBodyOO	 
]OO (
CreateCheckoutSessionRequestOO /
requestOO0 7
)OO7 8
{PP 
varQQ 
commandQQ 
=QQ 
newQQ (
CreateCheckoutSessionCommandQQ 6
(QQ6 7
requestRR 
.RR 
UserIdRR 
,RR 
requestSS 
.SS 
SubscriptionTypeSS $
)SS$ %
;SS% &
varUU 
resultUU 
=UU 
awaitUU 
	_mediatorUU $
.UU$ %
SendUU% )
(UU) *
commandUU* 1
)UU1 2
;UU2 3
returnWW 
resultWW 
.WW 
	IsSuccessWW 
?XX 

StatusCodeXX 
(XX 
resultXX 
.XX  

StatusCodeXX  *
,XX* +
resultXX, 2
.XX2 3
DataXX3 7
)XX7 8
:YY 

StatusCodeYY 
(YY 
resultYY 
.YY  

StatusCodeYY  *
,YY* +
newYY, /
{YY0 1
errorYY2 7
=YY8 9
resultYY: @
.YY@ A
ErrorYYA F
}YYG H
)YYH I
;YYI J
}ZZ 
[dd 
HttpPostdd 
(dd 
$strdd 
)dd 
]dd 
[ee 
AllowAnonymousee 
]ee 
publicff 

asyncff 
Taskff 
<ff 
IActionResultff #
>ff# $
StripeWebhookff% 2
(ff2 3
)ff3 4
{gg 
usingjj 
varjj 
readerjj 
=jj 
newjj 
StreamReaderjj +
(jj+ ,
Requestjj, 3
.jj3 4
Bodyjj4 8
)jj8 9
;jj9 :
varkk 
payloadkk 
=kk 
awaitkk 
readerkk "
.kk" #
ReadToEndAsynckk# 1
(kk1 2
)kk2 3
;kk3 4
varmm 
signatureHeadermm 
=mm 
Requestmm %
.mm% &
Headersmm& -
[mm- .
$strmm. @
]mm@ A
.mmA B
ToStringmmB J
(mmJ K
)mmK L
;mmL M
ifoo 

(oo 
stringoo 
.oo 
IsNullOrWhiteSpaceoo %
(oo% &
signatureHeaderoo& 5
)oo5 6
)oo6 7
{pp 	
returnqq 

BadRequestqq 
(qq 
newqq !
{qq" #
errorqq$ )
=qq* +
$strqq, N
}qqO P
)qqP Q
;qqQ R
}rr 	
vartt 
commandtt 
=tt 
newtt &
HandleStripeWebhookCommandtt 4
(tt4 5
payloadtt5 <
,tt< =
signatureHeadertt> M
)ttM N
;ttN O
varuu 
resultuu 
=uu 
awaituu 
	_mediatoruu $
.uu$ %
Senduu% )
(uu) *
commanduu* 1
)uu1 2
;uu2 3
returnww 
resultww 
.ww 
	IsSuccessww 
?xx 
Okxx 
(xx 
)xx 
:yy 

StatusCodeyy 
(yy 
resultyy 
.yy  

StatusCodeyy  *
,yy* +
newyy, /
{yy0 1
erroryy2 7
=yy8 9
resultyy: @
.yy@ A
ErroryyA F
}yyG H
)yyH I
;yyI J
}zz 
}{{ åF
QD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.API\Controllers\UsersController.cs
	namespace 	
AiTutor
 
. 
API 
. 
Controllers !
;! "
[ 
	Authorize 

]
 
public 
class 
UsersController 
: 
BaseController -
{ 
[ 
HttpGet 
] 
[ 
	Authorize 
( 
Roles 
= 
$str 
) 
]  
public 

async 
Task 
< 
IActionResult #
># $
GetAll% +
(+ ,
[, -
	FromQuery- 6
]6 7
int8 ;

pageNumber< F
=G H
$numI J
,J K
[ 	
	FromQuery	 
] 
int 
pageSize  
=! "
$num# %
)% &
{ 
var 
result 
= 
await 
Mediator #
.# $
Send$ (
(( )
new) ,
GetAllUsersQuery- =
(= >

pageNumber> H
,H I
pageSizeJ R
)R S
)S T
;T U
return 
Ok 
( 
result 
. 
Data 
) 
; 
} 
[ 
HttpGet 
( 
$str 
) 
] 
public 

async 
Task 
< 
IActionResult #
># $
GetById% ,
(, -
Guid- 1
id2 4
)4 5
{ 
var 
result 
= 
await 
Mediator #
.# $
Send$ (
(( )
new) ,
GetUserByIdQuery- =
(= >
id> @
)@ A
)A B
;B C
return 
Ok 
( 
result 
. 
Data 
) 
; 
} 
["" 
HttpGet"" 
("" 
$str"" .
)"". /
]""/ 0
public## 

async## 
Task## 
<## 
IActionResult## #
>### $
GetInvitationCode##% 6
(##6 7
Guid##7 ;
parentId##< D
)##D E
{$$ 
var%% 
result%% 
=%% 
await%% 
Mediator%% #
.%%# $
Send%%$ (
(%%( )
new%%) ,"
GetInvitationCodeQuery%%- C
(%%C D
parentId%%D L
)%%L M
)%%M N
;%%N O
return&& 
result&& 
.&& 
	IsSuccess&& 
?'' 
Ok'' 
('' 
result'' 
.'' 
Data'' 
)'' 
:(( 

StatusCode(( 
((( 
result(( 
.((  

StatusCode((  *
,((* +
new((, /
{((0 1
message((2 9
=((: ;
result((< B
.((B C
Error((C H
}((I J
)((J K
;((K L
})) 
[,, 
HttpGet,, 
(,, 
$str,, .
),,. /
],,/ 0
public-- 

async-- 
Task-- 
<-- 
IActionResult-- #
>--# $
GetChildren--% 0
(--0 1
Guid--1 5
parentId--6 >
)--> ?
{.. 
var// 
result// 
=// 
await// 
Mediator// #
.//# $
Send//$ (
(//( )
new//) ,
GetChildrenQuery//- =
(//= >
parentId//> F
)//F G
)//G H
;//H I
return00 
Ok00 
(00 
result00 
.00 
Data00 
)00 
;00 
}11 
[44 
HttpPost44 
(44 
$str44 
)44 
]44 
public55 

async55 
Task55 
<55 
IActionResult55 #
>55# $

LinkParent55% /
(55/ 0
[550 1
FromBody551 9
]559 :
LinkParentRequest55; L
request55M T
)55T U
{66 
var77 
result77 
=77 
await77 
Mediator77 #
.77# $
Send77$ (
(77( )
new77) ,
LinkParentCommand77- >
(77> ?
request77? F
.77F G
	StudentId77G P
,77P Q
request77R Y
.77Y Z
InvitationCode77Z h
)77h i
)77i j
;77j k
return88 
result88 
.88 
	IsSuccess88 
?99 
Ok99 
(99 
result99 
.99 
Data99 
)99 
::: 

StatusCode:: 
(:: 
result:: 
.::  

StatusCode::  *
,::* +
new::, /
{::0 1
message::2 9
=::: ;
result::< B
.::B C
Error::C H
}::I J
)::J K
;::K L
};; 
[>> 
HttpGet>> 
(>> 
$str>> )
)>>) *
]>>* +
public?? 

async?? 
Task?? 
<?? 
IActionResult?? #
>??# $
GetMyParent??% 0
(??0 1
Guid??1 5
	studentId??6 ?
)??? @
{@@ 
varAA 
resultAA 
=AA 
awaitAA 
MediatorAA #
.AA# $
SendAA$ (
(AA( )
newAA) ,
GetMyParentQueryAA- =
(AA= >
	studentIdAA> G
)AAG H
)AAH I
;AAI J
returnBB 
resultBB 
.BB 
	IsSuccessBB 
?CC 
OkCC 
(CC 
resultCC 
.CC 
DataCC 
)CC 
:DD 

StatusCodeDD 
(DD 
resultDD 
.DD  

StatusCodeDD  *
,DD* +
newDD, /
{DD0 1
messageDD2 9
=DD: ;
resultDD< B
.DDB C
ErrorDDC H
}DDI J
)DDJ K
;DDK L
}EE 
[HH 

HttpDeleteHH 
(HH 
$strHH ,
)HH, -
]HH- .
publicII 

asyncII 
TaskII 
<II 
IActionResultII #
>II# $
UnlinkParentII% 1
(II1 2
GuidII2 6
	studentIdII7 @
)II@ A
{JJ 
varKK 
resultKK 
=KK 
awaitKK 
MediatorKK #
.KK# $
SendKK$ (
(KK( )
newKK) ,
UnlinkParentCommandKK- @
(KK@ A
	studentIdKKA J
)KKJ K
)KKK L
;KKL M
returnLL 
resultLL 
.LL 
	IsSuccessLL 
?MM 
	NoContentMM 
(MM 
)MM 
:NN 

StatusCodeNN 
(NN 
resultNN 
.NN  

StatusCodeNN  *
,NN* +
newNN, /
{NN0 1
messageNN2 9
=NN: ;
resultNN< B
.NNB C
ErrorNNC H
}NNI J
)NNJ K
;NNK L
}OO 
[RR 
HttpGetRR 
(RR 
$strRR .
)RR. /
]RR/ 0
publicSS 

asyncSS 
TaskSS 
<SS 
IActionResultSS #
>SS# $
GetStudentGradesSS% 5
(SS5 6
GuidSS6 :
	studentIdSS; D
,SSD E
[SSF G
	FromQuerySSG P
]SSP Q
GuidSSR V
requesterIdSSW b
)SSb c
{TT 
varUU 
resultUU 
=UU 
awaitUU 
MediatorUU #
.UU# $
SendUU$ (
(UU( )
newUU) ,!
GetStudentGradesQueryUU- B
(UUB C
	studentIdUUC L
,UUL M
requesterIdUUN Y
)UUY Z
)UUZ [
;UU[ \
returnVV 
resultVV 
.VV 
	IsSuccessVV 
?WW 
OkWW 
(WW 
resultWW 
.WW 
DataWW 
)WW 
:XX 

StatusCodeXX 
(XX 
resultXX 
.XX  

StatusCodeXX  *
,XX* +
newXX, /
{XX0 1
messageXX2 9
=XX: ;
resultXX< B
.XXB C
ErrorXXC H
}XXI J
)XXJ K
;XXK L
}YY 
public]] 

record]] 
LinkParentRequest]] #
(]]# $
Guid]]$ (
	StudentId]]) 2
,]]2 3
string]]4 :
InvitationCode]]; I
)]]I J
;]]J K
}^^ Ï8
\D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.API\Middleware\ExceptionHandlingMiddleware.cs
	namespace 	
AiTutor
 
. 
API 
. 

Middleware  
;  !
public		 
class		 '
ExceptionHandlingMiddleware		 (
{

 
private 
readonly 
RequestDelegate $
_next% *
;* +
private 
readonly 
ILogger 
< '
ExceptionHandlingMiddleware 8
>8 9
_logger: A
;A B
public 
'
ExceptionHandlingMiddleware &
(& '
RequestDelegate' 6
next7 ;
,; <
ILogger 
< '
ExceptionHandlingMiddleware +
>+ ,
logger- 3
)3 4
{ 
_next 
= 
next 
; 
_logger 
= 
logger 
; 
} 
public 

async 
Task 
InvokeAsync !
(! "
HttpContext" -
context. 5
)5 6
{ 
try 
{ 	
await 
_next 
( 
context 
)  
;  !
} 	
catch 
( 
	Exception 
ex 
) 
{ 	
_logger 
. 
LogError 
( 
ex 
,  
$str! S
,S T
ex 
. 
GetType 
( 
) 
. 
FullName %
)% &
;& '
await  
HandleExceptionAsync &
(& '
context' .
,. /
ex0 2
)2 3
;3 4
}   	
}!! 
private## 
static## 
async## 
Task##  
HandleExceptionAsync## 2
(##2 3
HttpContext##3 >
context##? F
,##F G
	Exception##H Q
	exception##R [
)##[ \
{$$ 
context%% 
.%% 
Response%% 
.%% 
ContentType%% $
=%%% &
$str%%' 9
;%%9 :
int'' 

statusCode'' 
;'' 
object(( 
response(( 
;(( 
if** 

(** 
	exception** 
is** 
ValidationException** ,
validationEx**- 9
)**9 :
{++ 	

statusCode,, 
=,, 
(,, 
int,, 
),, 
HttpStatusCode,, ,
.,,, -

BadRequest,,- 7
;,,7 8
response-- 
=-- 
new-- 
{.. 
status// 
=// 

statusCode// #
,//# $
message00 
=00 
$str00 -
,00- .
errors11 
=11 
validationEx11 %
.11% &
Errors11& ,
}22 
;22 
}33 	
else44 
if44 
(44 
	exception44 
is44 
NotFoundException44 /

notFoundEx440 :
)44: ;
{55 	

statusCode66 
=66 
(66 
int66 
)66 
HttpStatusCode66 ,
.66, -
NotFound66- 5
;665 6
response77 
=77 
new77 
{88 
status99 
=99 

statusCode99 #
,99# $
message:: 
=:: 

notFoundEx:: $
.::$ %
Message::% ,
,::, -
errors;; 
=;; 
new;; 
{;; 
};;  
}<< 
;<< 
}== 	
else>> 
if>> 
(>> 
	exception>> 
is>> $
ForbiddenAccessException>> 6
)>>6 7
{?? 	

statusCode@@ 
=@@ 
(@@ 
int@@ 
)@@ 
HttpStatusCode@@ ,
.@@, -
	Forbidden@@- 6
;@@6 7
responseAA 
=AA 
newAA 
{BB 
statusCC 
=CC 

statusCodeCC #
,CC# $
messageDD 
=DD 
$strDD -
,DD- .
errorsEE 
=EE 
newEE 
{EE 
}EE  
}FF 
;FF 
}GG 	
elseHH 
ifHH 
(HH 
	exceptionHH 
isHH 
DomainExceptionHH -
domainExHH. 6
)HH6 7
{II 	

statusCodeJJ 
=JJ 
(JJ 
intJJ 
)JJ 
HttpStatusCodeJJ ,
.JJ, -

BadRequestJJ- 7
;JJ7 8
responseKK 
=KK 
newKK 
{LL 
statusMM 
=MM 

statusCodeMM #
,MM# $
messageNN 
=NN 
domainExNN "
.NN" #
MessageNN# *
,NN* +
errorsOO 
=OO 
newOO 
{OO 
}OO  
}PP 
;PP 
}QQ 	
elseRR 
ifRR 
(RR 
	exceptionRR 
isRR 
DbUpdateExceptionRR /
dbExRR0 4
)RR4 5
{SS 	

statusCodeTT 
=TT 
(TT 
intTT 
)TT 
HttpStatusCodeTT ,
.TT, -

BadRequestTT- 7
;TT7 8
responseUU 
=UU 
newUU 
{VV 
statusWW 
=WW 

statusCodeWW #
,WW# $
messageXX 
=XX 
dbExXX 
.XX 
InnerExceptionXX -
?XX- .
.XX. /
MessageXX/ 6
??XX7 9
dbExXX: >
.XX> ?
MessageXX? F
,XXF G
errorsYY 
=YY 
newYY 
{YY 
}YY  
}ZZ 
;ZZ 
}[[ 	
else\\ 
{]] 	

statusCode^^ 
=^^ 
(^^ 
int^^ 
)^^ 
HttpStatusCode^^ ,
.^^, -
InternalServerError^^- @
;^^@ A
response__ 
=__ 
new__ 
{`` 
statusaa 
=aa 

statusCodeaa #
,aa# $
messagebb 
=bb 
$strbb 9
,bb9 :
errorscc 
=cc 
newcc 
{cc 
}cc  
}dd 
;dd 
}ee 	
contextgg 
.gg 
Responsegg 
.gg 

StatusCodegg #
=gg$ %

statusCodegg& 0
;gg0 1
awaithh 
contexthh 
.hh 
Responsehh 
.hh 

WriteAsynchh )
(hh) *
JsonSerializerhh* 8
.hh8 9
	Serializehh9 B
(hhB C
responsehhC K
,hhK L
newii !
JsonSerializerOptionsii %
{ii& ' 
PropertyNamingPolicyii( <
=ii= >
JsonNamingPolicyii? O
.iiO P
	CamelCaseiiP Y
}iiZ [
)ii[ \
)ii\ ]
;ii] ^
}jj 
}kk £A
=D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.API\Program.cs
var 
builder 
= 
WebApplication 
. 
CreateBuilder *
(* +
args+ /
)/ 0
;0 1
builder 
. 
Services 
. 
AddApplication 
(  
)  !
;! "
builder 
. 
Services 
. 
AddInfrastructure "
(" #
builder# *
.* +
Configuration+ 8
)8 9
;9 :
builder 
. 
Services 
. 
AddControllers 
(  
)  !
;! "
builder 
. 
Services 
. #
AddEndpointsApiExplorer (
(( )
)) *
;* +
builder 
. 
Services 
. 
AddSwaggerGen 
( 
c  
=>! #
{ 
c 
. 

SwaggerDoc 
( 
$str 
, 
new 
OpenApiInfo &
{ 
Title 
= 
$str 
, 
Version 
= 
$str 
, 
Description 
= 
$str :
} 
) 
; 
c!! 
.!! !
AddSecurityDefinition!! 
(!! 
$str!! $
,!!$ %
new!!& )!
OpenApiSecurityScheme!!* ?
{"" 
Description## 
=## 
$str## I
,##I J
Name$$ 
=$$ 
$str$$ 
,$$ 
In%% 

=%% 
ParameterLocation%% 
.%% 
Header%% %
,%%% &
Type&& 
=&& 
SecuritySchemeType&& !
.&&! "
ApiKey&&" (
,&&( )
Scheme'' 
='' 
$str'' 
}(( 
)(( 
;(( 
c** 
.** "
AddSecurityRequirement** 
(** 
new**  &
OpenApiSecurityRequirement**! ;
{++ 
{,, 	
new-- !
OpenApiSecurityScheme-- %
{.. 
	Reference// 
=// 
new// 
OpenApiReference//  0
{00 
Type11 
=11 
ReferenceType11 (
.11( )
SecurityScheme11) 7
,117 8
Id22 
=22 
$str22 !
}33 
}44 
,44 
Array55 
.55 
Empty55 
<55 
string55 
>55 
(55  
)55  !
}66 	
}77 
)77 
;77 
}88 
)88 
;88 
var;; 
jwtSettings;; 
=;; 
builder;; 
.;; 
Configuration;; '
.;;' (

GetSection;;( 2
(;;2 3
$str;;3 @
);;@ A
;;;A B
var<< 
key<< 
=<< 	
Encoding<<
 
.<< 
UTF8<< 
.<< 
GetBytes<<  
(<<  !
jwtSettings<<! ,
[<<, -
$str<<- 8
]<<8 9
!<<9 :
)<<: ;
;<<; <
builder>> 
.>> 
Services>> 
.>> 
AddAuthentication>> "
(>>" #
options>># *
=>>>+ -
{?? 
options@@ 
.@@ %
DefaultAuthenticateScheme@@ %
=@@& '
JwtBearerDefaults@@( 9
.@@9 : 
AuthenticationScheme@@: N
;@@N O
optionsAA 
.AA "
DefaultChallengeSchemeAA "
=AA# $
JwtBearerDefaultsAA% 6
.AA6 7 
AuthenticationSchemeAA7 K
;AAK L
}BB 
)BB 
.CC 
AddJwtBearerCC 
(CC 
optionsCC 
=>CC 
{DD 
optionsEE 
.EE %
TokenValidationParametersEE %
=EE& '
newEE( +%
TokenValidationParametersEE, E
{FF $
ValidateIssuerSigningKeyGG  
=GG! "
trueGG# '
,GG' (
IssuerSigningKeyHH 
=HH 
newHH  
SymmetricSecurityKeyHH 3
(HH3 4
keyHH4 7
)HH7 8
,HH8 9
ValidateIssuerII 
=II 
trueII 
,II 
ValidIssuerJJ 
=JJ 
jwtSettingsJJ !
[JJ! "
$strJJ" *
]JJ* +
,JJ+ ,
ValidateAudienceKK 
=KK 
trueKK 
,KK  
ValidAudienceLL 
=LL 
jwtSettingsLL #
[LL# $
$strLL$ .
]LL. /
,LL/ 0
	ClockSkewMM 
=MM 
TimeSpanMM 
.MM 
ZeroMM !
}NN 
;NN 
}OO 
)OO 
;OO 
builderQQ 
.QQ 
ServicesQQ 
.QQ 
AddAuthorizationQQ !
(QQ! "
)QQ" #
;QQ# $
builderTT 
.TT 
ServicesTT 
.TT 
AddCorsTT 
(TT 
optionsTT  
=>TT! #
{UU 
optionsVV 
.VV 
	AddPolicyVV 
(VV 
$strVV  
,VV  !
policyVV" (
=>VV) +
{WW 
policyXX 
.XX 
AllowAnyOriginXX 
(XX 
)XX 
.YY 
AllowAnyMethodYY 
(YY 
)YY 
.ZZ 
AllowAnyHeaderZZ 
(ZZ 
)ZZ 
;ZZ  
}[[ 
)[[ 
;[[ 
}\\ 
)\\ 
;\\ 
var^^ 
app^^ 
=^^ 	
builder^^
 
.^^ 
Build^^ 
(^^ 
)^^ 
;^^ 
appaa 
.aa 
UseMiddlewareaa 
<aa '
ExceptionHandlingMiddlewareaa -
>aa- .
(aa. /
)aa/ 0
;aa0 1
ifcc 
(cc 
appcc 
.cc 
Environmentcc 
.cc 
IsDevelopmentcc !
(cc! "
)cc" #
)cc# $
{dd 
appee 
.ee 

UseSwaggeree 
(ee 
)ee 
;ee 
appff 
.ff 
UseSwaggerUIff 
(ff 
)ff 
;ff 
}gg 
appii 
.ii 
UseCorsii 
(ii 
$strii 
)ii 
;ii 
appjj 
.jj 
UseAuthenticationjj 
(jj 
)jj 
;jj 
appkk 
.kk 
UseAuthorizationkk 
(kk 
)kk 
;kk 
appll 
.ll 
MapControllersll 
(ll 
)ll 
;ll 
usingoo 
(oo 
varoo 

scopeoo 
=oo 
appoo 
.oo 
Servicesoo 
.oo  
CreateScopeoo  +
(oo+ ,
)oo, -
)oo- .
{pp 
varqq 
dbqq 

=qq 
scopeqq 
.qq 
ServiceProviderqq "
.qq" #
GetRequiredServiceqq# 5
<qq5 6 
ApplicationDbContextqq6 J
>qqJ K
(qqK L
)qqL M
;qqM N
awaitrr 	
dbrr
 
.rr 
Databaserr 
.rr 
MigrateAsyncrr "
(rr" #
)rr# $
;rr$ %
vartt 
userManagertt 
=tt 
scopett 
.tt 
ServiceProvidertt +
.tt+ ,
GetRequiredServicett, >
<tt> ?
UserManagertt? J
<ttJ K
ApplicationUserttK Z
>ttZ [
>tt[ \
(tt\ ]
)tt] ^
;tt^ _
awaituu 	
SeedDatauu
 
.uu 
	SeedAsyncuu 
(uu 
dbuu 
,uu  
userManageruu! ,
)uu, -
;uu- .
}vv 
appxx 
.xx 
Runxx 
(xx 
)xx 	
;xx	 

public{{ 
partial{{ 
class{{ 
Program{{ 
{{{ 
}{{  