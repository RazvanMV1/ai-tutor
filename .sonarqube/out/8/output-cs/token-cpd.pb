
^D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Common\Behaviors\LoggingBehavior.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Common $
.$ %
	Behaviors% .
;. /
public 
class 
LoggingBehavior 
< 
TRequest %
,% &
	TResponse' 0
>0 1
:2 3
IPipelineBehavior4 E
<E F
TRequestF N
,N O
	TResponseP Y
>Y Z
where 	
TRequest
 
: 
notnull 
{ 
private		 
readonly		 
ILogger		 
<		 
LoggingBehavior		 ,
<		, -
TRequest		- 5
,		5 6
	TResponse		7 @
>		@ A
>		A B
_logger		C J
;		J K
public 

LoggingBehavior 
( 
ILogger "
<" #
LoggingBehavior# 2
<2 3
TRequest3 ;
,; <
	TResponse= F
>F G
>G H
loggerI O
)O P
{ 
_logger 
= 
logger 
; 
} 
public 

async 
Task 
< 
	TResponse 
>  
Handle! '
(' (
TRequest( 0
request1 8
,8 9"
RequestHandlerDelegate: P
<P Q
	TResponseQ Z
>Z [
next\ `
,` a
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
requestName 
= 
typeof  
(  !
TRequest! )
)) *
.* +
Name+ /
;/ 0
_logger 
. 
LogInformation 
( 
$str 7
,7 8
requestName9 D
)D E
;E F
var 
response 
= 
await 
next !
(! "
)" #
;# $
_logger 
. 
LogInformation 
( 
$str 6
,6 7
requestName8 C
)C D
;D E
return 
response 
; 
} 
} Ÿ
aD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Common\Behaviors\ValidationBehavior.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Common $
.$ %
	Behaviors% .
;. /
public 
class 
ValidationBehavior 
<  
TRequest  (
,( )
	TResponse* 3
>3 4
:5 6
IPipelineBehavior7 H
<H I
TRequestI Q
,Q R
	TResponseS \
>\ ]
where 	
TRequest
 
: 
notnull 
{		 
private

 
readonly

 
IEnumerable

  
<

  !

IValidator

! +
<

+ ,
TRequest

, 4
>

4 5
>

5 6
_validators

7 B
;

B C
public 

ValidationBehavior 
( 
IEnumerable )
<) *

IValidator* 4
<4 5
TRequest5 =
>= >
>> ?

validators@ J
)J K
{ 
_validators 
= 

validators  
;  !
} 
public 

async 
Task 
< 
	TResponse 
>  
Handle! '
(' (
TRequest( 0
request1 8
,8 9"
RequestHandlerDelegate: P
<P Q
	TResponseQ Z
>Z [
next\ `
,` a
CancellationToken 
cancellationToken +
)+ ,
{ 
if 

( 
! 
_validators 
. 
Any 
( 
) 
) 
return 
await 
next 
( 
) 
;  
var 
context 
= 
new 
ValidationContext +
<+ ,
TRequest, 4
>4 5
(5 6
request6 =
)= >
;> ?
var 
validationResults 
= 
await  %
Task& *
.* +
WhenAll+ 2
(2 3
_validators 
. 
Select 
( 
v  
=>! #
v$ %
.% &
ValidateAsync& 3
(3 4
context4 ;
,; <
cancellationToken= N
)N O
)O P
)P Q
;Q R
var 
failures 
= 
validationResults (
. 

SelectMany 
( 
r 
=> 
r 
. 
Errors %
)% &
. 
Where 
( 
f 
=> 
f 
!= 
null !
)! "
. 
ToList 
( 
) 
; 
if   

(   
failures   
.   
Count   
!=   
$num   
)    
throw!! 
new!! 
ValidationException!! )
(!!) *
failures!!* 2
)!!2 3
;!!3 4
return## 
await## 
next## 
(## 
)## 
;## 
}$$ 
}%% Á
hD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Common\Exceptions\ForbiddenAccessException.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Common $
.$ %

Exceptions% /
;/ 0
public 
class $
ForbiddenAccessException %
:& '
	Exception( 1
{ 
public 
$
ForbiddenAccessException #
(# $
)$ %
:& '
base( ,
(, -
$str- b
)b c
{d e
}f g
} Œ
aD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Common\Exceptions\NotFoundException.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Common $
.$ %

Exceptions% /
;/ 0
public 
class 
NotFoundException 
:  
	Exception! *
{ 
public 

NotFoundException 
( 
string #
name$ (
,( )
object* 0
key1 4
)4 5
: 	
base
 
( 
$" 
$str 
{ 
name 
}  
$str  $
{$ %
key% (
}( )
$str) 9
"9 :
): ;
{< =
}> ?
} æ
cD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Common\Exceptions\ValidationException.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Common $
.$ %

Exceptions% /
;/ 0
public 
class 
ValidationException  
:! "
	Exception# ,
{ 
public 

IDictionary 
< 
string 
, 
string %
[% &
]& '
>' (
Errors) /
{0 1
get2 5
;5 6
}7 8
public		 

ValidationException		 
(		 
IEnumerable		 *
<		* +
ValidationFailure		+ <
>		< =
failures		> F
)		F G
:		H I
base		J N
(		N O
$str		O 
)			 Ä
{

 
Errors 
= 
failures 
. 
GroupBy 
( 
e 
=> 
e 
. 
PropertyName (
,( )
e* +
=>, .
e/ 0
.0 1
ErrorMessage1 =
)= >
. 
ToDictionary 
( 
failureGroup &
=>' )
failureGroup* 6
.6 7
Key7 :
,: ;
failureGroup< H
=>I K
failureGroupL X
.X Y
ToArrayY `
(` a
)a b
)b c
;c d
} 
} ®
_D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Common\Interfaces\IAiTutorService.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Common $
.$ %

Interfaces% /
;/ 0
public 
record 
ExplanationRequest  
(  !
string 

Topic 
, 
SubjectType 
Subject 
, 
DifficultyLevel 
DifficultyLevel #
,# $
int		 

StudentAge		 
)

 
;

 
public 
record 
ExplanationResponse !
(! "
string 

Topic 
, 
string 

Explanation 
, 
List 
< 	
string	 
> 
Examples 
, 
List 
< 	
string	 
> 
	KeyPoints 
, 
SubjectType 
Subject 
, 
DifficultyLevel 
DifficultyLevel #
) 
; 
public 
record 
HintRequest 
( 
string 

Question 
, 
SubjectType 
Subject 
) 
; 
public 
record 
HintResponse 
( 
string 

Question 
, 
string 

Hint 
, 
SubjectType 
Subject 
) 
; 
public   
record   
ProblemRequest   
(   
string!! 

Topic!! 
,!! 
SubjectType"" 
Subject"" 
,"" 
DifficultyLevel## 
DifficultyLevel## #
,### $
int$$ 

StudentAge$$ 
,$$ 
int%% 
Count%% 
)&& 
;&& 
public(( 
record(( 
ProblemResponse(( 
((( 
string)) 

Topic)) 
,)) 
List** 
<** 	
string**	 
>** 
Problems** 
,** 
List++ 
<++ 	
string++	 
>++ 
Hints++ 
,++ 
List,, 
<,, 	
string,,	 
>,, 
	Solutions,, 
,,, 
SubjectType-- 
Subject-- 
,-- 
DifficultyLevel.. 
DifficultyLevel.. #
)// 
;// 
public11 
	interface11 
IAiTutorService11  
{22 
Task33 
<33 	
ExplanationResponse33	 
>33 
GetExplanationAsync33 1
(331 2
ExplanationRequest332 D
request33E L
,33L M
CancellationToken33N _
ct33` b
=33c d
default33e l
)33l m
;33m n
Task44 
<44 	
HintResponse44	 
>44 
GetHintAsync44 #
(44# $
HintRequest44$ /
request440 7
,447 8
CancellationToken449 J
ct44K M
=44N O
default44P W
)44W X
;44X Y
Task55 
<55 	
ProblemResponse55	 
>55 !
GenerateProblemsAsync55 /
(55/ 0
ProblemRequest550 >
request55? F
,55F G
CancellationToken55H Y
ct55Z \
=55] ^
default55_ f
)55f g
;55g h
}66 ∫
eD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Common\Interfaces\IApplicationDbContext.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Common $
.$ %

Interfaces% /
;/ 0
public 
	interface !
IApplicationDbContext &
{ 
DbSet 	
<	 

User
 
> 
Users 
{ 
get 
; 
} 
DbSet		 	
<			 

Subject		
 
>		 
Subjects		 
{		 
get		 !
;		! "
}		# $
DbSet

 	
<

	 

Lesson


 
>

 
Lessons

 
{

 
get

 
;

  
}

! "
DbSet 	
<	 

Quiz
 
> 
Quizzes 
{ 
get 
; 
}  
DbSet 	
<	 

Question
 
> 
	Questions 
{ 
get  #
;# $
}% &
DbSet 	
<	 

StudentProgress
 
> 
StudentProgresses ,
{- .
get/ 2
;2 3
}4 5
DbSet 	
<	 

Subscription
 
> 
Subscriptions %
{& '
get( +
;+ ,
}- .
DbSet 	
<	 

	Classroom
 
> 

Classrooms 
{  !
get" %
;% &
}' (
DbSet 	
<	 

ClassroomMember
 
> 
ClassroomMembers +
{, -
get. 1
;1 2
}3 4
DbSet 	
<	 

ClassroomLesson
 
> 
ClassroomLessons +
{, -
get. 1
;1 2
}3 4
DbSet 	
<	 

ClassroomQuiz
 
> 
ClassroomQuizzes )
{* +
get, /
;/ 0
}1 2
DbSet 	
<	 

ClassroomQuestion
 
> 
ClassroomQuestions /
{0 1
get2 5
;5 6
}7 8
DbSet 	
<	 

ClassroomProgress
 
> 
ClassroomProgresses 0
{1 2
get3 6
;6 7
}8 9
DbSet 	
<	 

Grade
 
> 
Grades 
{ 
get 
; 
}  
Task 
< 	
int	 
> 
SaveChangesAsync 
( 
CancellationToken 0
cancellationToken1 B
=C D
defaultE L
)L M
;M N
} »
cD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Common\Interfaces\ICurrentUserService.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Common $
.$ %

Interfaces% /
;/ 0
public 
	interface 
ICurrentUserService $
{ 
Guid 
? 	
UserId
 
{ 
get 
; 
} 
string 

?
 
Email 
{ 
get 
; 
} 
bool 
IsAuthenticated	 
{ 
get 
; 
}  !
} Ù
[D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Common\Interfaces\IJwtService.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Common $
.$ %

Interfaces% /
;/ 0
public 
	interface 
IJwtService 
{ 
string 

GenerateToken 
( 
User 
user "
)" #
;# $
Guid 
? 	
ValidateToken
 
( 
string 
token $
)$ %
;% &
}		 Ó
^D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Common\Interfaces\IStripeService.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Common $
.$ %

Interfaces% /
;/ 0
public

 
	interface

 
IStripeService

 
{ 
Task 
< 	!
CheckoutSessionResult	 
> &
CreateCheckoutSessionAsync  :
(: ;
Guid 
userId 
, 
string 
	userEmail 
, 
SubscriptionType 
subscriptionType )
,) *
CancellationToken 
ct 
= 
default &
)& '
;' (
Task #
CancelSubscriptionAsync	  
(  !
string  
stripeSubscriptionId #
,# $
bool 
cancelImmediately 
=  
false! &
,& '
CancellationToken 
ct 
= 
default &
)& '
;' (
StripeWebhookEvent## !
ConstructWebhookEvent## ,
(##, -
string##- 3
payload##4 ;
,##; <
string##= C
signatureHeader##D S
)##S T
;##T U
string)) 

GetPriceIdFor)) 
()) 
SubscriptionType)) )
subscriptionType))* :
))): ;
;)); <
}** ≤
YD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Common\Models\PaginatedList.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Common $
.$ %
Models% +
;+ ,
public 
class 
PaginatedList 
< 
T 
> 
{ 
public 

List 
< 
T 
> 
Items 
{ 
get 
; 
}  !
public 

int 

PageNumber 
{ 
get 
;  
}! "
public 

int 

TotalPages 
{ 
get 
;  
}! "
public 

int 

TotalCount 
{ 
get 
;  
}! "
public		 

bool		 
HasPreviousPage		 
=>		  "

PageNumber		# -
>		. /
$num		0 1
;		1 2
public

 

bool

 
HasNextPage

 
=>

 

PageNumber

 )
<

* +

TotalPages

, 6
;

6 7
public 

PaginatedList 
( 
List 
< 
T 
>  
items! &
,& '
int( +
count, 1
,1 2
int3 6

pageNumber7 A
,A B
intC F
pageSizeG O
)O P
{ 
Items 
= 
items 
; 

TotalCount 
= 
count 
; 

PageNumber 
= 

pageNumber 
;  

TotalPages 
= 
( 
int 
) 
Math 
. 
Ceiling &
(& '
count' ,
/- .
(/ 0
double0 6
)6 7
pageSize7 ?
)? @
;@ A
} 
} —
RD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Common\Models\Result.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Common $
.$ %
Models% +
;+ ,
public 
class 
Result 
< 
T 
> 
{ 
public 

bool 
	IsSuccess 
{ 
get 
;  
private! (
set) ,
;, -
}. /
public 

T 
? 
Data 
{ 
get 
; 
private !
set" %
;% &
}' (
public 

string 
? 
Error 
{ 
get 
; 
private  '
set( +
;+ ,
}- .
public 

int 

StatusCode 
{ 
get 
;  
private! (
set) ,
;, -
}. /
private

 
Result

 
(

 
)

 
{

 
}

 
public 

static 
Result 
< 
T 
> 
Success #
(# $
T$ %
data& *
,* +
int, /

statusCode0 :
=; <
$num= @
)@ A
=>B D
new 
( 
) 
{ 
	IsSuccess 
= 
true  
,  !
Data" &
=' (
data) -
,- .

StatusCode/ 9
=: ;

statusCode< F
}G H
;H I
public 

static 
Result 
< 
T 
> 
Failure #
(# $
string$ *
error+ 0
,0 1
int2 5

statusCode6 @
=A B
$numC F
)F G
=>H J
new 
( 
) 
{ 
	IsSuccess 
= 
false !
,! "
Error# (
=) *
error+ 0
,0 1

StatusCode2 <
== >

statusCode? I
}J K
;K L
} Ÿ
_D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Common\Models\Stripe\StripeModels.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Common $
.$ %
Models% +
.+ ,
Stripe, 2
;2 3
public 
record !
CheckoutSessionResult #
(# $
string		 

	SessionId		 
,		 
string

 

CheckoutUrl

 
,

 
string 

StripeCustomerId 
, 
string 

StripePriceId 
) 
; 
public 
record 
StripeWebhookEvent  
(  !
string 

Id 
, 
string 

Type 
, "
StripeSubscriptionData 
? 
Subscription (
,( )%
StripeCheckoutSessionData 
? 
CheckoutSession .
). /
;/ 0
public 
record "
StripeSubscriptionData $
($ %
string   

SubscriptionId   
,   
string!! 


CustomerId!! 
,!! 
string"" 

PriceId"" 
,"" 
string## 

Status## 
,## 
DateTime$$ 
CurrentPeriodStart$$ 
,$$  
DateTime%% 
CurrentPeriodEnd%% 
,%% 
Guid&& 
?&& 	
	AppUserId&&
 
,&& 
SubscriptionType'' 
?'' 
AppSubscriptionType'' )
)'') *
;''* +
public,, 
record,, %
StripeCheckoutSessionData,, '
(,,' (
string-- 

	SessionId-- 
,-- 
string.. 


CustomerId.. 
,.. 
string// 

SubscriptionId// 
,// 
Guid00 
?00 	
	AppUserId00
 
,00 
SubscriptionType11 
?11 
AppSubscriptionType11 )
)11) *
;11* +
public66 
static66 
class66 
StripeStatusMapper66 &
{77 
public88 

static88 
SubscriptionStatus88 $
ToDomainStatus88% 3
(883 4
string884 :
stripeStatus88; G
)88G H
=>88I K
stripeStatus99 
?99 
.99 
ToLowerInvariant99 &
(99& '
)99' (
switch99) /
{:: 	
$str;; 
or;; 
$str;; "
=>;;# %
SubscriptionStatus;;& 8
.;;8 9
Active;;9 ?
,;;? @
$str<< 
=><< 
SubscriptionStatus<< ,
.<<, -
PastDue<<- 4
,<<4 5
$str== 
or== 
$str== "
=>==# %
SubscriptionStatus==& 8
.==8 9
Canceled==9 A
,==A B
$str>> 
or>> 
$str>> 0
=>>>1 3
SubscriptionStatus>>4 F
.>>F G

Incomplete>>G Q
,>>Q R
_?? 
=>?? 
SubscriptionStatus?? #
.??# $
Pending??$ +
}@@ 	
;@@	 

}AA í
QD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\DependencyInjection.cs
	namespace 	
AiTutor
 
. 
Application 
; 
public 
static 
class 
DependencyInjection '
{		 
public

 

static

 
IServiceCollection

 $
AddApplication

% 3
(

3 4
this

4 8
IServiceCollection

9 K
services

L T
)

T U
{ 
services 
. 

AddMediatR 
( 
cfg 
=>  "
cfg 
. (
RegisterServicesFromAssembly ,
(, -
typeof- 3
(3 4
DependencyInjection4 G
)G H
.H I
AssemblyI Q
)Q R
)R S
;S T
services 
. %
AddValidatorsFromAssembly *
(* +
typeof+ 1
(1 2
DependencyInjection2 E
)E F
.F G
AssemblyG O
)O P
;P Q
services 
. 
AddTransient 
( 
typeof $
($ %
IPipelineBehavior% 6
<6 7
,7 8
>8 9
)9 :
,: ;
typeof< B
(B C
ValidationBehaviorC U
<U V
,V W
>W X
)X Y
)Y Z
;Z [
services 
. 
AddTransient 
( 
typeof $
($ %
IPipelineBehavior% 6
<6 7
,7 8
>8 9
)9 :
,: ;
typeof< B
(B C
LoggingBehaviorC R
<R S
,S T
>T U
)U V
)V W
;W X
return 
services 
; 
} 
} Ø
xD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\AI\Queries\GenerateProblems\GenerateProblemsQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
AI' )
.) *
Queries* 1
.1 2
GenerateProblems2 B
;B C
public 
record !
GenerateProblemsQuery #
(# $
string		 

Topic		 
,		 
SubjectType

 
Subject

 
,

 
DifficultyLevel 
DifficultyLevel #
,# $
int 

StudentAge 
, 
int 
Count 
= 
$num 
) 
: 
IRequest 
< 
Result 
< 
ProblemResponse #
># $
>$ %
;% &«
D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\AI\Queries\GenerateProblems\GenerateProblemsQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
AI' )
.) *
Queries* 1
.1 2
GenerateProblems2 B
;B C
public 
class (
GenerateProblemsQueryHandler )
:* +
IRequestHandler, ;
<; <!
GenerateProblemsQuery< Q
,Q R
ResultS Y
<Y Z
ProblemResponseZ i
>i j
>j k
{ 
private		 
readonly		 
IAiTutorService		 $

_aiService		% /
;		/ 0
public 
(
GenerateProblemsQueryHandler '
(' (
IAiTutorService( 7
	aiService8 A
)A B
=> 


_aiService 
= 
	aiService !
;! "
public 

async 
Task 
< 
Result 
< 
ProblemResponse ,
>, -
>- .
Handle/ 5
(5 6!
GenerateProblemsQuery6 K
requestL S
,S T
CancellationTokenU f
ctg i
)i j
{ 
var 
response 
= 
await 

_aiService '
.' (!
GenerateProblemsAsync( =
(= >
new> A
ProblemRequestB P
(P Q
request 
. 
Topic 
, 
request 
. 
Subject 
, 
request 
. 
DifficultyLevel #
,# $
request 
. 

StudentAge 
, 
request 
. 
Count 
) 	
,	 

ct 
) 
; 
return 
Result 
< 
ProblemResponse %
>% &
.& '
Success' .
(. /
response/ 7
)7 8
;8 9
} 
} ◊
tD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\AI\Queries\GetExplanation\GetExplanationQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
AI' )
.) *
Queries* 1
.1 2
GetExplanation2 @
;@ A
public 
record 
GetExplanationQuery !
(! "
string		 

Topic		 
,		 
SubjectType

 
Subject

 
,

 
DifficultyLevel 
DifficultyLevel #
,# $
int 

StudentAge 
) 
: 
IRequest 
< 
Result 
< 
ExplanationResponse '
>' (
>( )
;) *Å
{D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\AI\Queries\GetExplanation\GetExplanationQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
AI' )
.) *
Queries* 1
.1 2
GetExplanation2 @
;@ A
public 
class &
GetExplanationQueryHandler '
:( )
IRequestHandler* 9
<9 :
GetExplanationQuery: M
,M N
ResultO U
<U V
ExplanationResponseV i
>i j
>j k
{ 
private		 
readonly		 
IAiTutorService		 $

_aiService		% /
;		/ 0
public 
&
GetExplanationQueryHandler %
(% &
IAiTutorService& 5
	aiService6 ?
)? @
=> 


_aiService 
= 
	aiService !
;! "
public 

async 
Task 
< 
Result 
< 
ExplanationResponse 0
>0 1
>1 2
Handle3 9
(9 :
GetExplanationQuery: M
requestN U
,U V
CancellationTokenW h
cti k
)k l
{ 
var 
response 
= 
await 

_aiService '
.' (
GetExplanationAsync( ;
(; <
new< ?
ExplanationRequest@ R
(R S
request 
. 
Topic 
, 
request 
. 
Subject 
, 
request 
. 
DifficultyLevel #
,# $
request 
. 

StudentAge 
) 	
,	 

ct 
) 
; 
return 
Result 
< 
ExplanationResponse )
>) *
.* +
Success+ 2
(2 3
response3 ;
); <
;< =
} 
} ∂
fD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\AI\Queries\GetHint\GetHintQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
AI' )
.) *
Queries* 1
.1 2
GetHint2 9
;9 :
public 
record 
GetHintQuery 
( 
string		 

Question		 
,		 
SubjectType

 
Subject

 
) 
: 
IRequest 
< 
Result 
< 
HintResponse  
>  !
>! "
;" #ï
mD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\AI\Queries\GetHint\GetHintQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
AI' )
.) *
Queries* 1
.1 2
GetHint2 9
;9 :
public 
class 
GetHintQueryHandler  
:! "
IRequestHandler# 2
<2 3
GetHintQuery3 ?
,? @
ResultA G
<G H
HintResponseH T
>T U
>U V
{ 
private		 
readonly		 
IAiTutorService		 $

_aiService		% /
;		/ 0
public 

GetHintQueryHandler 
( 
IAiTutorService .
	aiService/ 8
)8 9
=> 


_aiService 
= 
	aiService !
;! "
public 

async 
Task 
< 
Result 
< 
HintResponse )
>) *
>* +
Handle, 2
(2 3
GetHintQuery3 ?
request@ G
,G H
CancellationTokenI Z
ct[ ]
)] ^
{ 
var 
response 
= 
await 

_aiService '
.' (
GetHintAsync( 4
(4 5
new5 8
HintRequest9 D
(D E
request 
. 
Question 
, 
request 
. 
Subject 
) 	
,	 

ct 
) 
; 
return 
Result 
< 
HintResponse "
>" #
.# $
Success$ +
(+ ,
response, 4
)4 5
;5 6
} 
} ˛
ãD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\AddClassroomQuestion\AddClassroomQuestionCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ; 
AddClassroomQuestion; O
;O P
public 
record '
AddClassroomQuestionCommand )
() *
Guid 
ClassroomId	 
, 
Guid 
QuizId	 
, 
Guid		 
	TeacherId			 
,		 
string

 

Text

 
,

 
string 

CorrectAnswer 
, 
List 
< 	
string	 
> 
Options 
, 
int 
Points 
, 
string 

?
 
Explanation 
) 
: 
IRequest 
< 
Result 
< %
ClassroomQuestionResponse -
>- .
>. /
;/ 0
public 
record %
ClassroomQuestionResponse '
(' (
Guid 
Id	 
, 
Guid 
ClassroomQuizId	 
, 
string 

Text 
, 
List 
< 	
string	 
> 
Options 
, 
int 
Points 
, 
DateTime 
	CreatedAt 
) 
; ò%
íD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\AddClassroomQuestion\AddClassroomQuestionCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ; 
AddClassroomQuestion; O
;O P
public

 
class

 .
"AddClassroomQuestionCommandHandler

 /
:

0 1
IRequestHandler

2 A
<

A B'
AddClassroomQuestionCommand

B ]
,

] ^
Result

_ e
<

e f%
ClassroomQuestionResponse

f 
>	

 Ä
>


Ä Å
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
.
"AddClassroomQuestionCommandHandler -
(- .!
IApplicationDbContext. C
contextD K
)K L
=>M O
_contextP X
=Y Z
context[ b
;b c
public 

async 
Task 
< 
Result 
< %
ClassroomQuestionResponse 6
>6 7
>7 8
Handle9 ?
(? @'
AddClassroomQuestionCommand@ [
request\ c
,c d
CancellationTokene v
ctw y
)y z
{ 
var 
quiz 
= 
await 
_context !
.! "
ClassroomQuizzes" 2
. 
FirstOrDefaultAsync  
(  !
q! "
=># %
q& '
.' (
Id( *
==+ -
request. 5
.5 6
QuizId6 <
,< =
ct> @
)@ A
?? 
throw 
new 
NotFoundException *
(* +
nameof+ 1
(1 2
ClassroomQuiz2 ?
)? @
,@ A
requestB I
.I J
QuizIdJ P
)P Q
;Q R
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
?? 
throw 
new 
NotFoundException *
(* +
nameof+ 1
(1 2
	Classroom2 ;
); <
,< =
request> E
.E F
ClassroomIdF Q
)Q R
;R S
if 

( 
	classroom 
. 
	TeacherId 
!=  "
request# *
.* +
	TeacherId+ 4
)4 5
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
var 
question 
= 
ClassroomQuestion (
.( )
Create) /
(/ 0
request 
. 
QuizId 
, 
request #
.# $
Text$ (
,( )
request* 1
.1 2
CorrectAnswer2 ?
,? @
request 
. 
Options 
, 
request $
.$ %
Points% +
,+ ,
request- 4
.4 5
Explanation5 @
)@ A
;A B
_context   
.   
ClassroomQuestions   #
.  # $
Add  $ '
(  ' (
question  ( 0
)  0 1
;  1 2
await!! 
_context!! 
.!! 
SaveChangesAsync!! '
(!!' (
ct!!( *
)!!* +
;!!+ ,
return## 
Result## 
<## %
ClassroomQuestionResponse## /
>##/ 0
.##0 1
Success##1 8
(##8 9
new##9 <%
ClassroomQuestionResponse##= V
(##V W
question$$ 
.$$ 
Id$$ 
,$$ 
question$$ !
.$$! "
ClassroomQuizId$$" 1
,$$1 2
question$$3 ;
.$$; <
Text$$< @
,$$@ A
question%% 
.%% 
Options%% 
,%% 
question%% &
.%%& '
Points%%' -
,%%- .
question%%/ 7
.%%7 8
	CreatedAt%%8 A
)%%A B
,%%B C
$num%%D G
)%%G H
;%%H I
}&& 
}'' í
sD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\AddGrade\AddGradeCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
AddGrade; C
;C D
public 
record 
AddGradeCommand 
( 
Guid 
ClassroomId	 
, 
Guid 
	StudentId	 
, 
Guid		 
	TeacherId			 
,		 
int

 
Value

 
,

 
string 

Description 
) 
: 
IRequest 
< 
Result 
< 
GradeResponse !
>! "
>" #
;# $
public 
record 
GradeResponse 
( 
Guid 
Id	 
, 
Guid 
ClassroomId	 
, 
Guid 
	StudentId	 
, 
string 

StudentName 
, 
Guid 
	TeacherId	 
, 
int 
Value 
, 
string 

Description 
, 
DateTime 
GradedAt 
) 
; ú-
zD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\AddGrade\AddGradeCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
AddGrade; C
;C D
public

 
class

 "
AddGradeCommandHandler

 #
:

$ %
IRequestHandler

& 5
<

5 6
AddGradeCommand

6 E
,

E F
Result

G M
<

M N
GradeResponse

N [
>

[ \
>

\ ]
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
"
AddGradeCommandHandler !
(! "!
IApplicationDbContext" 7
context8 ?
)? @
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
GradeResponse *
>* +
>+ ,
Handle- 3
(3 4
AddGradeCommand4 C
requestD K
,K L
CancellationTokenM ^
ct_ a
)a b
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
if 

( 
	classroom 
. 
	TeacherId 
!=  "
request# *
.* +
	TeacherId+ 4
)4 5
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
var 
student 
= 
await 
_context $
.$ %
Users% *
. 
FirstOrDefaultAsync  
(  !
u! "
=># %
u& '
.' (
Id( *
==+ -
request. 5
.5 6
	StudentId6 ?
,? @
ctA C
)C D
;D E
if 

( 
student 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
User/ 3
)3 4
,4 5
request6 =
.= >
	StudentId> G
)G H
;H I
var 
isMember 
= 
await 
_context %
.% &
ClassroomMembers& 6
.   
AnyAsync   
(   
m   
=>   
m   
.   
ClassroomId   (
==  ) +
request  , 3
.  3 4
ClassroomId  4 ?
&&  @ B
m!! 
.!! 
	StudentId!! %
==!!& (
request!!) 0
.!!0 1
	StudentId!!1 :
&&!!; =
m!!> ?
.!!? @
IsActive!!@ H
,!!H I
ct!!J L
)!!L M
;!!M N
if"" 

("" 
!"" 
isMember"" 
)"" 
return## 
Result## 
<## 
GradeResponse## '
>##' (
.##( )
Failure##) 0
(##0 1
$str##1 S
,##S T
$num##U X
)##X Y
;##Y Z
var%% 
grade%% 
=%% 
Grade%% 
.%% 
Create%%  
(%%  !
request&& 
.&& 
ClassroomId&& 
,&&  
request'' 
.'' 
	StudentId'' 
,'' 
request(( 
.(( 
	TeacherId(( 
,(( 
request)) 
.)) 
Value)) 
,)) 
request** 
.** 
Description** 
)++ 	
;++	 

_context-- 
.-- 
Grades-- 
.-- 
Add-- 
(-- 
grade-- !
)--! "
;--" #
await.. 
_context.. 
... 
SaveChangesAsync.. '
(..' (
ct..( *
)..* +
;..+ ,
return00 
Result00 
<00 
GradeResponse00 #
>00# $
.00$ %
Success00% ,
(00, -
new00- 0
GradeResponse001 >
(00> ?
grade11 
.11 
Id11 
,11 
grade11 
.11 
ClassroomId11 '
,11' (
grade11) .
.11. /
	StudentId11/ 8
,118 9
student22 
.22 
FullName22 
,22 
grade22 #
.22# $
	TeacherId22$ -
,22- .
grade33 
.33 
Value33 
,33 
grade33 
.33 
Description33 *
,33* +
grade33, 1
.331 2
GradedAt332 :
)44 	
,44	 

$num44 
)44 
;44 
}55 
}66 á
uD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\AddMember\AddMemberCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
	AddMember; D
;D E
public 
record 
AddMemberCommand 
( 
Guid #
ClassroomId$ /
,/ 0
Guid1 5
	TeacherId6 ?
,? @
stringA G
StudentEmailH T
)T U
: 
IRequest 
< 
Result 
< 
bool 
> 
> 
; ¶-
|D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\AddMember\AddMemberCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
	AddMember; D
;D E
public

 
class

 #
AddMemberCommandHandler

 $
:

% &
IRequestHandler

' 6
<

6 7
AddMemberCommand

7 G
,

G H
Result

I O
<

O P
bool

P T
>

T U
>

U V
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
#
AddMemberCommandHandler "
(" #!
IApplicationDbContext# 8
context9 @
)@ A
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
bool !
>! "
>" #
Handle$ *
(* +
AddMemberCommand+ ;
request< C
,C D
CancellationTokenE V
ctW Y
)Y Z
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
Include 
( 
c 
=> 
c 
. 
Members #
)# $
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
if 

( 
	classroom 
. 
	TeacherId 
!=  "
request# *
.* +
	TeacherId+ 4
)4 5
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
var 

emailLower 
= 
request  
.  !
StudentEmail! -
.- .
ToLowerInvariant. >
(> ?
)? @
;@ A
var 
allUsers 
= 
await 
_context %
.% &
Users& +
.+ ,
ToListAsync, 7
(7 8
ct8 :
): ;
;; <
var 
student 
= 
allUsers 
. 
FirstOrDefault -
(- .
u. /
=>0 2
u3 4
.4 5
Email5 :
.: ;
Value; @
==A C

emailLowerD N
)N O
;O P
if 

( 
student 
is 
null 
) 
return 
Result 
< 
bool 
> 
.  
Failure  '
(' (
$str( U
,U V
$numW Z
)Z [
;[ \
var!! 
activeMembers!! 
=!! 
	classroom!! %
.!!% &
Members!!& -
.!!- .
Count!!. 3
(!!3 4
m!!4 5
=>!!6 8
m!!9 :
.!!: ;
IsActive!!; C
)!!C D
;!!D E
if"" 

("" 
activeMembers"" 
>="" 
	classroom"" &
.""& '
MaxStudents""' 2
)""2 3
return## 
Result## 
<## 
bool## 
>## 
.##  
Failure##  '
(##' (
$str##( L
,##L M
$num##N Q
)##Q R
;##R S
var%% 
alreadyMember%% 
=%% 
	classroom%% %
.%%% &
Members%%& -
.&& 
Any&& 
(&& 
m&& 
=>&& 
m&& 
.&& 
	StudentId&& !
==&&" $
student&&% ,
.&&, -
Id&&- /
&&&&0 2
m&&3 4
.&&4 5
IsActive&&5 =
)&&= >
;&&> ?
if'' 

('' 
alreadyMember'' 
)'' 
return(( 
Result(( 
<(( 
bool(( 
>(( 
.((  
Failure((  '
(((' (
$str((( L
,((L M
$num((N Q
)((Q R
;((R S
var** 
member** 
=** 
ClassroomMember** $
.**$ %
Create**% +
(**+ ,
	classroom**, 5
.**5 6
Id**6 8
,**8 9
student**: A
.**A B
Id**B D
)**D E
;**E F
_context++ 
.++ 
ClassroomMembers++ !
.++! "
Add++" %
(++% &
member++& ,
)++, -
;++- .
await,, 
_context,, 
.,, 
SaveChangesAsync,, '
(,,' (
ct,,( *
),,* +
;,,+ ,
return.. 
Result.. 
<.. 
bool.. 
>.. 
... 
Success.. #
(..# $
true..$ (
)..( )
;..) *
}// 
}00 Ó
çD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\CreateClassroomLesson\CreateClassroomLessonCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;!
CreateClassroomLesson; P
;P Q
public 
record (
CreateClassroomLessonCommand *
(* +
Guid 
ClassroomId	 
, 
Guid		 
	TeacherId			 
,		 
string

 

Title

 
,

 
string 

Content 
, 
int 

OrderIndex 
, 
DifficultyLevel 

Difficulty 
) 
: 
IRequest 
< 
Result 
< #
ClassroomLessonResponse +
>+ ,
>, -
;- .
public 
record #
ClassroomLessonResponse %
(% &
Guid 
Id	 
, 
Guid 
ClassroomId	 
, 
string 

Title 
, 
string 

Content 
, 
int 

OrderIndex 
, 
DifficultyLevel 

Difficulty 
, 
DateTime 
	CreatedAt 
) 
; Œ 
îD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\CreateClassroomLesson\CreateClassroomLessonCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;!
CreateClassroomLesson; P
;P Q
public

 
class

 /
#CreateClassroomLessonCommandHandler

 0
: 
IRequestHandler 
< (
CreateClassroomLessonCommand 2
,2 3
Result4 :
<: ;#
ClassroomLessonResponse; R
>R S
>S T
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
/
#CreateClassroomLessonCommandHandler .
(. /!
IApplicationDbContext/ D
contextE L
)L M
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< #
ClassroomLessonResponse 4
>4 5
>5 6
Handle7 =
(= >(
CreateClassroomLessonCommand $
request% ,
,, -
CancellationToken. ?
ct@ B
)B C
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
if 

( 
	classroom 
. 
	TeacherId 
!=  "
request# *
.* +
	TeacherId+ 4
)4 5
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
var 
lesson 
= 
ClassroomLesson $
.$ %
Create% +
(+ ,
request 
. 
ClassroomId 
,  
request 
. 
Title 
, 
request 
. 
Content 
, 
request   
.   

OrderIndex   
,   
request!! 
.!! 

Difficulty!! 
)"" 	
;""	 

_context$$ 
.$$ 
ClassroomLessons$$ !
.$$! "
Add$$" %
($$% &
lesson$$& ,
)$$, -
;$$- .
await%% 
_context%% 
.%% 
SaveChangesAsync%% '
(%%' (
ct%%( *
)%%* +
;%%+ ,
return'' 
Result'' 
<'' #
ClassroomLessonResponse'' -
>''- .
.''. /
Success''/ 6
(''6 7
new''7 :#
ClassroomLessonResponse''; R
(''R S
lesson(( 
.(( 
Id(( 
,(( 
lesson(( 
.(( 
ClassroomId(( )
,(() *
lesson((+ 1
.((1 2
Title((2 7
,((7 8
lesson((9 ?
.((? @
Content((@ G
,((G H
lesson)) 
.)) 

OrderIndex)) 
,)) 
lesson)) %
.))% &

Difficulty))& 0
,))0 1
lesson))2 8
.))8 9
	CreatedAt))9 B
)** 	
,**	 

$num** 
)** 
;** 
}++ 
},, ∞
âD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\CreateClassroomQuiz\CreateClassroomQuizCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
CreateClassroomQuiz; N
;N O
public 
record &
CreateClassroomQuizCommand (
(( )
Guid 
ClassroomId	 
, 
Guid		 
LessonId			 
,		 
Guid

 
	TeacherId

	 
,

 
string 

Title 
, 
DifficultyLevel 

Difficulty 
, 
int 
TimeLimitMinutes 
) 
: 
IRequest 
< 
Result 
< !
ClassroomQuizResponse )
>) *
>* +
;+ ,
public 
record !
ClassroomQuizResponse #
(# $
Guid 
Id	 
, 
Guid 
ClassroomLessonId	 
, 
Guid 
ClassroomId	 
, 
string 

Title 
, 
DifficultyLevel 

Difficulty 
, 
int 
TimeLimitMinutes 
, 
int 
QuestionCount 
, 
DateTime 
	CreatedAt 
) 
; µ&
êD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\CreateClassroomQuiz\CreateClassroomQuizCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
CreateClassroomQuiz; N
;N O
public

 
class

 -
!CreateClassroomQuizCommandHandler

 .
:

/ 0
IRequestHandler

1 @
<

@ A&
CreateClassroomQuizCommand

A [
,

[ \
Result

] c
<

c d!
ClassroomQuizResponse

d y
>

y z
>

z {
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
-
!CreateClassroomQuizCommandHandler ,
(, -!
IApplicationDbContext- B
contextC J
)J K
=>L N
_contextO W
=X Y
contextZ a
;a b
public 

async 
Task 
< 
Result 
< !
ClassroomQuizResponse 2
>2 3
>3 4
Handle5 ;
(; <&
CreateClassroomQuizCommand< V
requestW ^
,^ _
CancellationToken` q
ctr t
)t u
{ 
var 
lesson 
= 
await 
_context #
.# $
ClassroomLessons$ 4
. 
FirstOrDefaultAsync  
(  !
l! "
=># %
l& '
.' (
Id( *
==+ -
request. 5
.5 6
LessonId6 >
&&? A
lB C
.C D
ClassroomIdD O
==P R
requestS Z
.Z [
ClassroomId[ f
,f g
cth j
)j k
?? 
throw 
new 
NotFoundException *
(* +
nameof+ 1
(1 2
ClassroomLesson2 A
)A B
,B C
requestD K
.K L
LessonIdL T
)T U
;U V
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
?? 
throw 
new 
NotFoundException *
(* +
nameof+ 1
(1 2
	Classroom2 ;
); <
,< =
request> E
.E F
ClassroomIdF Q
)Q R
;R S
if 

( 
	classroom 
. 
	TeacherId 
!=  "
request# *
.* +
	TeacherId+ 4
)4 5
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
var 
quiz 
= 
ClassroomQuiz  
.  !
Create! '
(' (
request( /
./ 0
ClassroomId0 ;
,; <
request= D
.D E
LessonIdE M
,M N
requestO V
.V W
TitleW \
,\ ]
request^ e
.e f

Difficultyf p
,p q
requestr y
.y z
TimeLimitMinutes	z ä
)
ä ã
;
ã å
_context 
. 
ClassroomQuizzes !
.! "
Add" %
(% &
quiz& *
)* +
;+ ,
await 
_context 
. 
SaveChangesAsync '
(' (
ct( *
)* +
;+ ,
return 
Result 
< !
ClassroomQuizResponse +
>+ ,
., -
Success- 4
(4 5
new5 8!
ClassroomQuizResponse9 N
(N O
quiz 
. 
Id 
, 
quiz 
. 
ClassroomLessonId +
,+ ,
quiz- 1
.1 2
ClassroomId2 =
,= >
quiz? C
.C D
TitleD I
,I J
quiz 
. 

Difficulty 
, 
quiz !
.! "
TimeLimitMinutes" 2
,2 3
$num4 5
,5 6
quiz7 ;
.; <
	CreatedAt< E
)E F
,F G
$numH K
)K L
;L M
} 
} ˛
ÅD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\CreateClassroom\CreateClassroomCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
CreateClassroom; J
;J K
public 
record "
CreateClassroomCommand $
($ %
string 

Name 
, 
string		 

Description		 
,		 
SubjectType

 
SubjectType

 
,

 
Guid 
	TeacherId	 
) 
: 
IRequest 
< 
Result 
< 
ClassroomResponse %
>% &
>& '
;' (
public 
record 
ClassroomResponse 
(  
Guid 
Id	 
, 
string 

Name 
, 
string 

Description 
, 
SubjectType 
SubjectType 
, 
Guid 
	TeacherId	 
, 
string 

	ClassCode 
, 
int 
MaxStudents 
, 
bool 
IsActive	 
, 
int 
MemberCount 
, 
int 
LessonCount 
) 
; ñ,
àD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\CreateClassroom\CreateClassroomCommandHandler.cs
	namespace		 	
AiTutor		
 
.		 
Application		 
.		 
Features		 &
.		& '

Classrooms		' 1
.		1 2
Commands		2 :
.		: ;
CreateClassroom		; J
;		J K
public 
class )
CreateClassroomCommandHandler *
:+ ,
IRequestHandler- <
<< ="
CreateClassroomCommand= S
,S T
ResultU [
<[ \
ClassroomResponse\ m
>m n
>n o
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
)
CreateClassroomCommandHandler (
(( )!
IApplicationDbContext) >
context? F
)F G
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
ClassroomResponse .
>. /
>/ 0
Handle1 7
(7 8"
CreateClassroomCommand8 N
requestO V
,V W
CancellationTokenX i
ctj l
)l m
{ 
var 
teacher 
= 
await 
_context $
.$ %
Users% *
. 
FirstOrDefaultAsync  
(  !
u! "
=># %
u& '
.' (
Id( *
==+ -
request. 5
.5 6
	TeacherId6 ?
,? @
ctA C
)C D
;D E
if 

( 
teacher 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
User/ 3
)3 4
,4 5
request6 =
.= >
	TeacherId> G
)G H
;H I
var !
hasActiveSubscription !
=" #
await$ )
_context* 2
.2 3
Subscriptions3 @
. 
AnyAsync 
( 
s 
=> 
s 
. 
UserId #
==$ &
request' .
.. /
	TeacherId/ 8
&&9 ;
s 
. 
IsActive $
&&% '
s 
. 
EndDate #
>=$ &
DateTime' /
./ 0
UtcNow0 6
,6 7
ct8 :
): ;
;; <
if 

( 
! !
hasActiveSubscription "
)" #
return   
Result   
<   
ClassroomResponse   +
>  + ,
.  , -
Failure  - 4
(  4 5
$str  5 k
,  k l
$num  m p
)  p q
;  q r
var## 
classroomCount## 
=## 
await## "
_context### +
.##+ ,

Classrooms##, 6
.$$ 

CountAsync$$ 
($$ 
c$$ 
=>$$ 
c$$ 
.$$ 
	TeacherId$$ (
==$$) +
request$$, 3
.$$3 4
	TeacherId$$4 =
&&$$> @
c$$A B
.$$B C
IsActive$$C K
,$$K L
ct$$M O
)$$O P
;$$P Q
if%% 

(%% 
classroomCount%% 
>=%% 
$num%% 
)%%  
return&& 
Result&& 
<&& 
ClassroomResponse&& +
>&&+ ,
.&&, -
Failure&&- 4
(&&4 5
$str&&5 `
,&&` a
$num&&b e
)&&e f
;&&f g
var(( 
	classroom(( 
=(( 
	Classroom(( !
.((! "
Create((" (
(((( )
request)) 
.)) 
Name)) 
,)) 
request** 
.** 
Description** 
,**  
request++ 
.++ 
SubjectType++ 
,++  
request,, 
.,, 
	TeacherId,, 
)-- 	
;--	 

_context// 
.// 

Classrooms// 
.// 
Add// 
(//  
	classroom//  )
)//) *
;//* +
await00 
_context00 
.00 
SaveChangesAsync00 '
(00' (
ct00( *
)00* +
;00+ ,
return22 
Result22 
<22 
ClassroomResponse22 '
>22' (
.22( )
Success22) 0
(220 1
new221 4
ClassroomResponse225 F
(22F G
	classroom33 
.33 
Id33 
,33 
	classroom44 
.44 
Name44 
,44 
	classroom55 
.55 
Description55 !
,55! "
	classroom66 
.66 
SubjectType66 !
,66! "
	classroom77 
.77 
	TeacherId77 
,77  
	classroom88 
.88 
	ClassCode88 
,88  
	classroom99 
.99 
MaxStudents99 !
,99! "
	classroom:: 
.:: 
IsActive:: 
,:: 
$num;; 
,;; 
$num;; 
)<< 	
,<<	 

$num<< 
)<< 
;<< 
}== 
}>> Ω
äD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\CreateClassroom\CreateClassroomCommandValidator.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
CreateClassroom; J
;J K
public 
class +
CreateClassroomCommandValidator ,
:- .
AbstractValidator/ @
<@ A"
CreateClassroomCommandA W
>W X
{ 
public 
+
CreateClassroomCommandValidator *
(* +
)+ ,
{ 
RuleFor		 
(		 
x		 
=>		 
x		 
.		 
Name		 
)		 
.

 
NotEmpty

 
(

 
)

 
.

 
WithMessage

 #
(

# $
$str

$ E
)

E F
. 
MaximumLength 
( 
$num 
) 
.  
WithMessage  +
(+ ,
$str, S
)S T
;T U
RuleFor 
( 
x 
=> 
x 
. 
Description "
)" #
. 
MaximumLength 
( 
$num 
) 
.  
WithMessage  +
(+ ,
$str, W
)W X
;X Y
RuleFor 
( 
x 
=> 
x 
. 
	TeacherId  
)  !
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ A
)A B
;B C
} 
} ∞
çD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\DeleteClassroomLesson\DeleteClassroomLessonCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;!
DeleteClassroomLesson; P
;P Q
public 
record (
DeleteClassroomLessonCommand *
(* +
Guid 
ClassroomId	 
, 
Guid 
LessonId	 
, 
Guid		 
	TeacherId			 
)

 
:

 
IRequest

 
<

 
Result

 
<

 
bool

 
>

 
>

 
;

 È
îD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\DeleteClassroomLesson\DeleteClassroomLessonCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;!
DeleteClassroomLesson; P
;P Q
public

 
class

 /
#DeleteClassroomLessonCommandHandler

 0
: 
IRequestHandler 
< (
DeleteClassroomLessonCommand 2
,2 3
Result4 :
<: ;
bool; ?
>? @
>@ A
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
/
#DeleteClassroomLessonCommandHandler .
(. /!
IApplicationDbContext/ D
contextE L
)L M
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
bool !
>! "
>" #
Handle$ *
(* +(
DeleteClassroomLessonCommand $
request% ,
,, -
CancellationToken. ?
ct@ B
)B C
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
if 

( 
	classroom 
. 
	TeacherId 
!=  "
request# *
.* +
	TeacherId+ 4
)4 5
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
var 
lesson 
= 
await 
_context #
.# $
ClassroomLessons$ 4
. 
FirstOrDefaultAsync  
(  !
l! "
=># %
l& '
.' (
Id( *
==+ -
request. 5
.5 6
LessonId6 >
&&? A
l& '
.' (
ClassroomId( 3
==4 6
request7 >
.> ?
ClassroomId? J
,J K
ctL N
)N O
;O P
if 

( 
lesson 
is 
null 
) 
throw   
new   
NotFoundException   '
(  ' (
nameof  ( .
(  . /
ClassroomLesson  / >
)  > ?
,  ? @
request  A H
.  H I
LessonId  I Q
)  Q R
;  R S
_context"" 
."" 
ClassroomLessons"" !
.""! "
Remove""" (
(""( )
lesson"") /
)""/ 0
;""0 1
await## 
_context## 
.## 
SaveChangesAsync## '
(##' (
ct##( *
)##* +
;##+ ,
return%% 
Result%% 
<%% 
bool%% 
>%% 
.%% 
Success%% #
(%%# $
true%%$ (
)%%( )
;%%) *
}&& 
}'' Ô
ëD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\DeleteClassroomQuestion\DeleteClassroomQuestionCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;#
DeleteClassroomQuestion; R
;R S
public 
record *
DeleteClassroomQuestionCommand ,
(, -
Guid 
ClassroomId	 
, 
Guid 
QuizId	 
, 
Guid		 

QuestionId			 
,		 
Guid

 
	TeacherId

	 
) 
: 
IRequest 
< 
Result 
< 
bool 
> 
> 
; Ü
òD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\DeleteClassroomQuestion\DeleteClassroomQuestionCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;#
DeleteClassroomQuestion; R
;R S
public

 
class

 1
%DeleteClassroomQuestionCommandHandler

 2
: 
IRequestHandler 
< *
DeleteClassroomQuestionCommand 4
,4 5
Result6 <
<< =
bool= A
>A B
>B C
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
1
%DeleteClassroomQuestionCommandHandler 0
(0 1!
IApplicationDbContext1 F
contextG N
)N O
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
bool !
>! "
>" #
Handle$ *
(* +*
DeleteClassroomQuestionCommand &
request' .
,. /
CancellationToken0 A
ctB D
)D E
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
if 

( 
	classroom 
. 
	TeacherId 
!=  "
request# *
.* +
	TeacherId+ 4
)4 5
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
var 
question 
= 
await 
_context %
.% &
ClassroomQuestions& 8
. 
FirstOrDefaultAsync  
(  !
q! "
=># %
q& '
.' (
Id( *
==+ -
request. 5
.5 6

QuestionId6 @
&&A C
q& '
.' (
ClassroomQuizId( 7
==8 :
request; B
.B C
QuizIdC I
,I J
ctK M
)M N
;N O
if 

( 
question 
is 
null 
) 
throw   
new   
NotFoundException   '
(  ' (
nameof  ( .
(  . /
ClassroomQuestion  / @
)  @ A
,  A B
request  C J
.  J K

QuestionId  K U
)  U V
;  V W
_context"" 
."" 
ClassroomQuestions"" #
.""# $
Remove""$ *
(""* +
question""+ 3
)""3 4
;""4 5
await## 
_context## 
.## 
SaveChangesAsync## '
(##' (
ct##( *
)##* +
;##+ ,
return%% 
Result%% 
<%% 
bool%% 
>%% 
.%% 
Success%% #
(%%# $
true%%$ (
)%%( )
;%%) *
}&& 
}'' ¶
âD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\DeleteClassroomQuiz\DeleteClassroomQuizCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
DeleteClassroomQuiz; N
;N O
public 
record &
DeleteClassroomQuizCommand (
(( )
Guid 
ClassroomId	 
, 
Guid 
QuizId	 
, 
Guid		 
	TeacherId			 
)

 
:

 
IRequest

 
<

 
Result

 
<

 
bool

 
>

 
>

 
;

 œ
êD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\DeleteClassroomQuiz\DeleteClassroomQuizCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
DeleteClassroomQuiz; N
;N O
public

 
class

 -
!DeleteClassroomQuizCommandHandler

 .
: 
IRequestHandler 
< &
DeleteClassroomQuizCommand 0
,0 1
Result2 8
<8 9
bool9 =
>= >
>> ?
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
-
!DeleteClassroomQuizCommandHandler ,
(, -!
IApplicationDbContext- B
contextC J
)J K
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
bool !
>! "
>" #
Handle$ *
(* +&
DeleteClassroomQuizCommand "
request# *
,* +
CancellationToken, =
ct> @
)@ A
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
if 

( 
	classroom 
. 
	TeacherId 
!=  "
request# *
.* +
	TeacherId+ 4
)4 5
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
var 
quiz 
= 
await 
_context !
.! "
ClassroomQuizzes" 2
. 
FirstOrDefaultAsync  
(  !
q! "
=># %
q& '
.' (
Id( *
==+ -
request. 5
.5 6
QuizId6 <
&&= ?
q& '
.' (
ClassroomId( 3
==4 6
request7 >
.> ?
ClassroomId? J
,J K
ctL N
)N O
;O P
if 

( 
quiz 
is 
null 
) 
throw   
new   
NotFoundException   '
(  ' (
nameof  ( .
(  . /
ClassroomQuiz  / <
)  < =
,  = >
request  ? F
.  F G
QuizId  G M
)  M N
;  N O
_context"" 
."" 
ClassroomQuizzes"" !
.""! "
Remove""" (
(""( )
quiz"") -
)""- .
;"". /
await## 
_context## 
.## 
SaveChangesAsync## '
(##' (
ct##( *
)##* +
;##+ ,
return%% 
Result%% 
<%% 
bool%% 
>%% 
.%% 
Success%% #
(%%# $
true%%$ (
)%%( )
;%%) *
}&& 
}'' „
ÅD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\DeleteClassroom\DeleteClassroomCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
DeleteClassroom; J
;J K
public 
record "
DeleteClassroomCommand $
($ %
Guid% )
ClassroomId* 5
,5 6
Guid7 ;
	TeacherId< E
)E F
:G H
IRequestI Q
<Q R
ResultR X
<X Y
boolY ]
>] ^
>^ _
;_ `Ê
àD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\DeleteClassroom\DeleteClassroomCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
DeleteClassroom; J
;J K
public

 
class

 )
DeleteClassroomCommandHandler

 *
:

+ ,
IRequestHandler

- <
<

< ="
DeleteClassroomCommand

= S
,

S T
Result

U [
<

[ \
bool

\ `
>

` a
>

a b
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
)
DeleteClassroomCommandHandler (
(( )!
IApplicationDbContext) >
context? F
)F G
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
bool !
>! "
>" #
Handle$ *
(* +"
DeleteClassroomCommand+ A
requestB I
,I J
CancellationTokenK \
ct] _
)_ `
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
if 

( 
	classroom 
. 
	TeacherId 
!=  "
request# *
.* +
	TeacherId+ 4
)4 5
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
	classroom 
. 

Deactivate 
( 
) 
; 
await 
_context 
. 
SaveChangesAsync '
(' (
ct( *
)* +
;+ ,
return 
Result 
< 
bool 
> 
. 
Success #
(# $
true$ (
)( )
;) *
} 
} Œ
yD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\DeleteGrade\DeleteGradeCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
DeleteGrade; F
;F G
public 
record 
DeleteGradeCommand  
(  !
Guid! %
GradeId& -
,- .
Guid/ 3
	TeacherId4 =
)= >
:? @
IRequestA I
<I J
ResultJ P
<P Q
boolQ U
>U V
>V W
;W Xﬂ
ÄD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\DeleteGrade\DeleteGradeCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
DeleteGrade; F
;F G
public

 
class

 %
DeleteGradeCommandHandler

 &
:

' (
IRequestHandler

) 8
<

8 9
DeleteGradeCommand

9 K
,

K L
Result

M S
<

S T
bool

T X
>

X Y
>

Y Z
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
%
DeleteGradeCommandHandler $
($ %!
IApplicationDbContext% :
context; B
)B C
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
bool !
>! "
>" #
Handle$ *
(* +
DeleteGradeCommand+ =
request> E
,E F
CancellationTokenG X
ctY [
)[ \
{ 
var 
grade 
= 
await 
_context "
." #
Grades# )
. 
FirstOrDefaultAsync  
(  !
g! "
=># %
g& '
.' (
Id( *
==+ -
request. 5
.5 6
GradeId6 =
,= >
ct? A
)A B
;B C
if 

( 
grade 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
Grade/ 4
)4 5
,5 6
request7 >
.> ?
GradeId? F
)F G
;G H
if 

( 
grade 
. 
	TeacherId 
!= 
request &
.& '
	TeacherId' 0
)0 1
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
_context 
. 
Grades 
. 
Remove 
( 
grade $
)$ %
;% &
await 
_context 
. 
SaveChangesAsync '
(' (
ct( *
)* +
;+ ,
return 
Result 
< 
bool 
> 
. 
Success #
(# $
true$ (
)( )
;) *
} 
} ⁄
}D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\JoinClassroom\JoinClassroomCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
JoinClassroom; H
;H I
public 
record  
JoinClassroomCommand "
(" #
string# )
	ClassCode* 3
,3 4
Guid5 9
	StudentId: C
)C D
:E F
IRequestG O
<O P
ResultP V
<V W
boolW [
>[ \
>\ ]
;] ^ê#
ÑD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\JoinClassroom\JoinClassroomCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
JoinClassroom; H
;H I
public

 
class

 '
JoinClassroomCommandHandler

 (
:

) *
IRequestHandler

+ :
<

: ; 
JoinClassroomCommand

; O
,

O P
Result

Q W
<

W X
bool

X \
>

\ ]
>

] ^
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
'
JoinClassroomCommandHandler &
(& '!
IApplicationDbContext' <
context= D
)D E
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
bool !
>! "
>" #
Handle$ *
(* + 
JoinClassroomCommand+ ?
request@ G
,G H
CancellationTokenI Z
ct[ ]
)] ^
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
Include 
( 
c 
=> 
c 
. 
Members #
)# $
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
	ClassCode( 1
==2 4
request5 <
.< =
	ClassCode= F
&&G I
cJ K
.K L
IsActiveL T
,T U
ctV X
)X Y
;Y Z
if 

( 
	classroom 
is 
null 
) 
return 
Result 
< 
bool 
> 
.  
Failure  '
(' (
$str( a
,a b
$numc f
)f g
;g h
var 
activeMembers 
= 
	classroom %
.% &
Members& -
.- .
Count. 3
(3 4
m4 5
=>6 8
m9 :
.: ;
IsActive; C
)C D
;D E
if 

( 
activeMembers 
>= 
	classroom &
.& '
MaxStudents' 2
)2 3
return 
Result 
< 
bool 
> 
.  
Failure  '
(' (
$str( L
,L M
$numN Q
)Q R
;R S
var 
alreadyMember 
= 
	classroom %
.% &
Members& -
. 
Any 
( 
m 
=> 
m 
. 
	StudentId !
==" $
request% ,
., -
	StudentId- 6
&&7 9
m: ;
.; <
IsActive< D
)D E
;E F
if 

( 
alreadyMember 
) 
return   
Result   
<   
bool   
>   
.    
Failure    '
(  ' (
$str  ( M
,  M N
$num  O R
)  R S
;  S T
var"" 
member"" 
="" 
ClassroomMember"" $
.""$ %
Create""% +
(""+ ,
	classroom"", 5
.""5 6
Id""6 8
,""8 9
request"": A
.""A B
	StudentId""B K
)""K L
;""L M
_context## 
.## 
ClassroomMembers## !
.##! "
Add##" %
(##% &
member##& ,
)##, -
;##- .
await$$ 
_context$$ 
.$$ 
SaveChangesAsync$$ '
($$' (
ct$$( *
)$$* +
;$$+ ,
return&& 
Result&& 
<&& 
bool&& 
>&& 
.&& 
Success&& #
(&&# $
true&&$ (
)&&( )
;&&) *
}'' 
}(( é
{D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\RemoveMember\RemoveMemberCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
RemoveMember; G
;G H
public 
record 
RemoveMemberCommand !
(! "
Guid" &
ClassroomId' 2
,2 3
Guid4 8
	TeacherId9 B
,B C
GuidD H
	StudentIdI R
)R S
: 
IRequest 
< 
Result 
< 
bool 
> 
> 
; °*
ÇD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\RemoveMember\RemoveMemberCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
RemoveMember; G
;G H
public

 
class

 &
RemoveMemberCommandHandler

 '
:

( )
IRequestHandler

* 9
<

9 :
RemoveMemberCommand

: M
,

M N
Result

O U
<

U V
bool

V Z
>

Z [
>

[ \
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
&
RemoveMemberCommandHandler %
(% &!
IApplicationDbContext& ;
context< C
)C D
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
bool !
>! "
>" #
Handle$ *
(* +
RemoveMemberCommand+ >
request? F
,F G
CancellationTokenH Y
ctZ \
)\ ]
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
if 

( 
	classroom 
. 
	TeacherId 
!=  "
request# *
.* +
	TeacherId+ 4
)4 5
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
var 
member 
= 
await 
_context #
.# $
ClassroomMembers$ 4
. 
FirstOrDefaultAsync  
(  !
m! "
=># %
m& '
.' (
ClassroomId( 3
==4 6
request7 >
.> ?
ClassroomId? J
&&K M
m% &
.& '
	StudentId' 0
==1 3
request4 ;
.; <
	StudentId< E
&&F H
mI J
.J K
IsActiveK S
,S T
ctU W
)W X
;X Y
if 

( 
member 
is 
null 
) 
return 
Result 
< 
bool 
> 
.  
Failure  '
(' (
$str( J
,J K
$numL O
)O P
;P Q
member   
.   

Deactivate   
(   
)   
;   
var## 

progresses## 
=## 
await## 
_context## '
.##' (
ClassroomProgresses##( ;
.$$ 
Where$$ 
($$ 
p$$ 
=>$$ 
p$$ 
.$$ 
ClassroomId$$ %
==$$& (
request$$) 0
.$$0 1
ClassroomId$$1 <
&&$$= ?
p$$@ A
.$$A B
	StudentId$$B K
==$$L N
request$$O V
.$$V W
	StudentId$$W `
)$$` a
.%% 
ToListAsync%% 
(%% 
ct%% 
)%% 
;%% 
_context&& 
.&& 
ClassroomProgresses&& $
.&&$ %
RemoveRange&&% 0
(&&0 1

progresses&&1 ;
)&&; <
;&&< =
var(( 
grades(( 
=(( 
await(( 
_context(( #
.((# $
Grades(($ *
.)) 
Where)) 
()) 
g)) 
=>)) 
g)) 
.)) 
ClassroomId)) %
==))& (
request))) 0
.))0 1
ClassroomId))1 <
&&))= ?
g))@ A
.))A B
	StudentId))B K
==))L N
request))O V
.))V W
	StudentId))W `
)))` a
.** 
ToListAsync** 
(** 
ct** 
)** 
;** 
_context++ 
.++ 
Grades++ 
.++ 
RemoveRange++ #
(++# $
grades++$ *
)++* +
;+++ ,
await-- 
_context-- 
.-- 
SaveChangesAsync-- '
(--' (
ct--( *
)--* +
;--+ ,
return.. 
Result.. 
<.. 
bool.. 
>.. 
... 
Success.. #
(..# $
true..$ (
)..( )
;..) *
}// 
}00 ”
âD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\SubmitClassroomQuiz\SubmitClassroomQuizCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
SubmitClassroomQuiz; N
;N O
public 
record 
QuizAnswerDto 
( 
Guid  

QuestionId! +
,+ ,
string- 3
Answer4 :
): ;
;; <
public 
record &
SubmitClassroomQuizCommand (
(( )
Guid		 
ClassroomId			 
,		 
Guid

 
QuizId

	 
,

 
Guid 
	StudentId	 
, 
List 
< 	
QuizAnswerDto	 
> 
Answers 
) 
: 
IRequest 
< 
Result 
< '
ClassroomQuizResultResponse /
>/ 0
>0 1
;1 2
public 
record 
QuizQuestionResult  
(  !
Guid 

QuestionId	 
, 
string 

QuestionText 
, 
string 


UserAnswer 
, 
string 

CorrectAnswer 
, 
bool 
	IsCorrect	 
, 
string 

?
 
Explanation 
) 
; 
public 
record '
ClassroomQuizResultResponse )
() *
Guid 
QuizId	 
, 
Guid 
	StudentId	 
, 
int 
TotalQuestions 
, 
int 
CorrectAnswers 
, 
double 

ScorePercentage 
, 
List 
< 	
QuizQuestionResult	 
> 
Results $
) 
; á?
êD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\SubmitClassroomQuiz\SubmitClassroomQuizCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
SubmitClassroomQuiz; N
;N O
public

 
class

 -
!SubmitClassroomQuizCommandHandler

 .
:

/ 0
IRequestHandler

1 @
<

@ A&
SubmitClassroomQuizCommand

A [
,

[ \
Result

] c
<

c d'
ClassroomQuizResultResponse

d 
>	

 Ä
>


Ä Å
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
-
!SubmitClassroomQuizCommandHandler ,
(, -!
IApplicationDbContext- B
contextC J
)J K
=>L N
_contextO W
=X Y
contextZ a
;a b
public 

async 
Task 
< 
Result 
< '
ClassroomQuizResultResponse 8
>8 9
>9 :
Handle; A
(A B&
SubmitClassroomQuizCommandB \
request] d
,d e
CancellationTokenf w
ctx z
)z {
{ 
var 
isMember 
= 
await 
_context %
.% &
ClassroomMembers& 6
. 
AnyAsync 
( 
m 
=> 
m 
. 
ClassroomId (
==) +
request, 3
.3 4
ClassroomId4 ?
&&@ B
mC D
.D E
	StudentIdE N
==O Q
requestR Y
.Y Z
	StudentIdZ c
&&d f
mg h
.h i
IsActivei q
,q r
cts u
)u v
;v w
if 

( 
! 
isMember 
) 
throw 
new  $
ForbiddenAccessException! 9
(9 :
): ;
;; <
var 
quiz 
= 
await 
_context !
.! "
ClassroomQuizzes" 2
. 
Include 
( 
q 
=> 
q 
. 
	Questions %
)% &
. 
FirstOrDefaultAsync  
(  !
q! "
=># %
q& '
.' (
Id( *
==+ -
request. 5
.5 6
QuizId6 <
&&= ?
q@ A
.A B
ClassroomIdB M
==N P
requestQ X
.X Y
ClassroomIdY d
,d e
ctf h
)h i
?? 
throw 
new 
NotFoundException *
(* +
nameof+ 1
(1 2
ClassroomQuiz2 ?
)? @
,@ A
requestB I
.I J
QuizIdJ P
)P Q
;Q R
var 
results 
= 
quiz 
. 
	Questions $
.$ %
Select% +
(+ ,
q, -
=>. 0
{1 2
var 
answer 
= 
request  
.  !
Answers! (
.( )
FirstOrDefault) 7
(7 8
a8 9
=>: <
a= >
.> ?

QuestionId? I
==J L
qM N
.N O
IdO Q
)Q R
;R S
var 
	isCorrect 
= 
answer "
!=# %
null& *
&&+ -
q. /
./ 0
	IsCorrect0 9
(9 :
answer: @
.@ A
AnswerA G
)G H
;H I
return 
new 
QuizQuestionResult )
() *
q* +
.+ ,
Id, .
,. /
q0 1
.1 2
Text2 6
,6 7
answer8 >
?> ?
.? @
Answer@ F
??G I
$strJ L
,L M
qN O
.O P
CorrectAnswerP ]
,] ^
	isCorrect_ h
,h i
qj k
.k l
Explanationl w
)w x
;x y
} 	
)	 

.
 
ToList 
( 
) 
; 
var 
correct 
= 
results 
. 
Count #
(# $
r$ %
=>& (
r) *
.* +
	IsCorrect+ 4
)4 5
;5 6
var 
scoreDouble 
= 
quiz 
. 
	Questions (
.( )
Count) .
>/ 0
$num1 2
?3 4
Math5 9
.9 :
Round: ?
(? @
(@ A
doubleA G
)G H
correctH O
/P Q
quizR V
.V W
	QuestionsW `
.` a
Counta f
*g h
$numi l
,l m
$numn o
)o p
:q r
$nums t
;t u
var 
scoreInt 
= 
( 
int 
) 
Math  
.  !
Round! &
(& '
scoreDouble' 2
)2 3
;3 4
var 
progress 
= 
await 
_context %
.% &
ClassroomProgresses& 9
.   
FirstOrDefaultAsync    
(    !
p  ! "
=>  # %
p  & '
.  ' (
ClassroomId  ( 3
==  4 6
request  7 >
.  > ?
ClassroomId  ? J
&&  K M
p  N O
.  O P
	StudentId  P Y
==  Z \
request  ] d
.  d e
	StudentId  e n
&&  o q
p  r s
.  s t
ClassroomLessonId	  t Ö
==
  Ü à
quiz
  â ç
.
  ç é
ClassroomLessonId
  é ü
,
  ü †
ct
  ° £
)
  £ §
;
  § •
if!! 

(!! 
progress!! 
==!! 
null!! 
)!! 
{"" 	
progress## 
=## 
ClassroomProgress## (
.##( )
Create##) /
(##/ 0
request##0 7
.##7 8
ClassroomId##8 C
,##C D
request##E L
.##L M
	StudentId##M V
,##V W
quiz##X \
.##\ ]
ClassroomLessonId##] n
)##n o
;##o p
_context$$ 
.$$ 
ClassroomProgresses$$ (
.$$( )
Add$$) ,
($$, -
progress$$- 5
)$$5 6
;$$6 7
}%% 	
progress&& 
.&& 
RegisterAttempt&&  
(&&  !
scoreInt&&! )
)&&) *
;&&* +
await'' 
_context'' 
.'' 
SaveChangesAsync'' '
(''' (
ct''( *
)''* +
;''+ ,
return(( 
Result(( 
<(( '
ClassroomQuizResultResponse(( 1
>((1 2
.((2 3
Success((3 :
(((: ;
new((; >'
ClassroomQuizResultResponse((? Z
(((Z [
quiz)) 
.)) 
Id)) 
,)) 
request)) 
.)) 
	StudentId)) &
,))& '
quiz))( ,
.)), -
	Questions))- 6
.))6 7
Count))7 <
,))< =
correct))> E
,))E F
scoreDouble))G R
,))R S
results))T [
)))[ \
)))\ ]
;))] ^
}** 
}++ ı
çD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\UpdateClassroomLesson\UpdateClassroomLessonCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;!
UpdateClassroomLesson; P
;P Q
public 
record (
UpdateClassroomLessonCommand *
(* +
Guid		 
ClassroomId			 
,		 
Guid

 
LessonId

	 
,

 
Guid 
	TeacherId	 
, 
string 

Title 
, 
string 

Content 
, 
DifficultyLevel 

Difficulty 
) 
: 
IRequest 
< 
Result 
< #
ClassroomLessonResponse +
>+ ,
>, -
;- .“$
îD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\UpdateClassroomLesson\UpdateClassroomLessonCommandHandler.cs
	namespace		 	
AiTutor		
 
.		 
Application		 
.		 
Features		 &
.		& '

Classrooms		' 1
.		1 2
Commands		2 :
.		: ;!
UpdateClassroomLesson		; P
;		P Q
public 
class /
#UpdateClassroomLessonCommandHandler 0
: 
IRequestHandler 
< (
UpdateClassroomLessonCommand 2
,2 3
Result4 :
<: ;#
ClassroomLessonResponse; R
>R S
>S T
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
/
#UpdateClassroomLessonCommandHandler .
(. /!
IApplicationDbContext/ D
contextE L
)L M
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< #
ClassroomLessonResponse 4
>4 5
>5 6
Handle7 =
(= >(
UpdateClassroomLessonCommand $
request% ,
,, -
CancellationToken. ?
ct@ B
)B C
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
if 

( 
	classroom 
. 
	TeacherId 
!=  "
request# *
.* +
	TeacherId+ 4
)4 5
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
var 
lesson 
= 
await 
_context #
.# $
ClassroomLessons$ 4
. 
FirstOrDefaultAsync  
(  !
l! "
=># %
l& '
.' (
Id( *
==+ -
request. 5
.5 6
LessonId6 >
&&? A
l& '
.' (
ClassroomId( 3
==4 6
request7 >
.> ?
ClassroomId? J
,J K
ctL N
)N O
;O P
if   

(   
lesson   
is   
null   
)   
throw!! 
new!! 
NotFoundException!! '
(!!' (
nameof!!( .
(!!. /
ClassroomLesson!!/ >
)!!> ?
,!!? @
request!!A H
.!!H I
LessonId!!I Q
)!!Q R
;!!R S
lesson## 
.## 
Update## 
(## 
request## 
.## 
Title## #
,### $
request##% ,
.##, -
Content##- 4
,##4 5
request##6 =
.##= >

Difficulty##> H
)##H I
;##I J
await$$ 
_context$$ 
.$$ 
SaveChangesAsync$$ '
($$' (
ct$$( *
)$$* +
;$$+ ,
return&& 
Result&& 
<&& #
ClassroomLessonResponse&& -
>&&- .
.&&. /
Success&&/ 6
(&&6 7
new&&7 :#
ClassroomLessonResponse&&; R
(&&R S
lesson'' 
.'' 
Id'' 
,'' 
lesson'' 
.'' 
ClassroomId'' )
,'') *
lesson''+ 1
.''1 2
Title''2 7
,''7 8
lesson''9 ?
.''? @
Content''@ G
,''G H
lesson(( 
.(( 

OrderIndex(( 
,(( 
lesson(( %
.((% &

Difficulty((& 0
,((0 1
lesson((2 8
.((8 9
	CreatedAt((9 B
))) 	
)))	 

;))
 
}** 
}++ Ô
âD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\UpdateClassroomQuiz\UpdateClassroomQuizCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
UpdateClassroomQuiz; N
;N O
public 
record &
UpdateClassroomQuizCommand (
(( )
Guid		 
ClassroomId			 
,		 
Guid

 
QuizId

	 
,

 
Guid 
	TeacherId	 
, 
string 

Title 
, 
DifficultyLevel 

Difficulty 
, 
int 
TimeLimitMinutes 
) 
: 
IRequest 
< 
Result 
< !
ClassroomQuizResponse )
>) *
>* +
;+ ,∫&
êD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\UpdateClassroomQuiz\UpdateClassroomQuizCommandHandler.cs
	namespace		 	
AiTutor		
 
.		 
Application		 
.		 
Features		 &
.		& '

Classrooms		' 1
.		1 2
Commands		2 :
.		: ;
UpdateClassroomQuiz		; N
;		N O
public 
class -
!UpdateClassroomQuizCommandHandler .
: 
IRequestHandler 
< &
UpdateClassroomQuizCommand 0
,0 1
Result2 8
<8 9!
ClassroomQuizResponse9 N
>N O
>O P
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
-
!UpdateClassroomQuizCommandHandler ,
(, -!
IApplicationDbContext- B
contextC J
)J K
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< !
ClassroomQuizResponse 2
>2 3
>3 4
Handle5 ;
(; <&
UpdateClassroomQuizCommand "
request# *
,* +
CancellationToken, =
ct> @
)@ A
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
if 

( 
	classroom 
. 
	TeacherId 
!=  "
request# *
.* +
	TeacherId+ 4
)4 5
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
var 
quiz 
= 
await 
_context !
.! "
ClassroomQuizzes" 2
. 
Include 
( 
q 
=> 
q 
. 
	Questions %
)% &
. 
FirstOrDefaultAsync  
(  !
q! "
=># %
q& '
.' (
Id( *
==+ -
request. 5
.5 6
QuizId6 <
&&= ?
q  & '
.  ' (
ClassroomId  ( 3
==  4 6
request  7 >
.  > ?
ClassroomId  ? J
,  J K
ct  L N
)  N O
;  O P
if!! 

(!! 
quiz!! 
is!! 
null!! 
)!! 
throw"" 
new"" 
NotFoundException"" '
(""' (
nameof""( .
("". /
ClassroomQuiz""/ <
)""< =
,""= >
request""? F
.""F G
QuizId""G M
)""M N
;""N O
quiz$$ 
.$$ 
Update$$ 
($$ 
request$$ 
.$$ 
Title$$ !
,$$! "
request$$# *
.$$* +

Difficulty$$+ 5
,$$5 6
request$$7 >
.$$> ?
TimeLimitMinutes$$? O
)$$O P
;$$P Q
await%% 
_context%% 
.%% 
SaveChangesAsync%% '
(%%' (
ct%%( *
)%%* +
;%%+ ,
return'' 
Result'' 
<'' !
ClassroomQuizResponse'' +
>''+ ,
.'', -
Success''- 4
(''4 5
new''5 8!
ClassroomQuizResponse''9 N
(''N O
quiz(( 
.(( 
Id(( 
,(( 
quiz(( 
.(( 
ClassroomLessonId(( +
,((+ ,
quiz((- 1
.((1 2
ClassroomId((2 =
,((= >
quiz((? C
.((C D
Title((D I
,((I J
quiz)) 
.)) 

Difficulty)) 
,)) 
quiz)) !
.))! "
TimeLimitMinutes))" 2
,))2 3
quiz))4 8
.))8 9
	Questions))9 B
.))B C
Count))C H
,))H I
quiz))J N
.))N O
	CreatedAt))O X
)** 	
)**	 

;**
 
}++ 
},, Ó
ÅD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\UpdateClassroom\UpdateClassroomCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
UpdateClassroom; J
;J K
public 
record "
UpdateClassroomCommand $
($ %
Guid 
ClassroomId	 
, 
Guid		 
	TeacherId			 
,		 
string

 

Name

 
,

 
string 

?
 
Description 
) 
: 
IRequest 
< 
Result 
< 
ClassroomResponse %
>% &
>& '
;' (·$
àD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\UpdateClassroom\UpdateClassroomCommandHandler.cs
	namespace		 	
AiTutor		
 
.		 
Application		 
.		 
Features		 &
.		& '

Classrooms		' 1
.		1 2
Commands		2 :
.		: ;
UpdateClassroom		; J
;		J K
public 
class )
UpdateClassroomCommandHandler *
:+ ,
IRequestHandler- <
<< ="
UpdateClassroomCommand= S
,S T
ResultU [
<[ \
ClassroomResponse\ m
>m n
>n o
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
)
UpdateClassroomCommandHandler (
(( )!
IApplicationDbContext) >
context? F
)F G
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
ClassroomResponse .
>. /
>/ 0
Handle1 7
(7 8"
UpdateClassroomCommand8 N
requestO V
,V W
CancellationTokenX i
ctj l
)l m
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
if 

( 
	classroom 
. 
	TeacherId 
!=  "
request# *
.* +
	TeacherId+ 4
)4 5
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
	classroom 
. 
Update 
( 
request  
.  !
Name! %
,% &
request' .
.. /
Description/ :
): ;
;; <
await 
_context 
. 
SaveChangesAsync '
(' (
ct( *
)* +
;+ ,
var 
memberCount 
= 
await 
_context  (
.( )
ClassroomMembers) 9
. 

CountAsync 
( 
m 
=> 
m 
. 
ClassroomId *
==+ -
	classroom. 7
.7 8
Id8 :
&&; =
m> ?
.? @
IsActive@ H
,H I
ctJ L
)L M
;M N
var   
lessonCount   
=   
await   
_context    (
.  ( )
ClassroomLessons  ) 9
.!! 

CountAsync!! 
(!! 
l!! 
=>!! 
l!! 
.!! 
ClassroomId!! *
==!!+ -
	classroom!!. 7
.!!7 8
Id!!8 :
,!!: ;
ct!!< >
)!!> ?
;!!? @
return## 
Result## 
<## 
ClassroomResponse## '
>##' (
.##( )
Success##) 0
(##0 1
new##1 4
ClassroomResponse##5 F
(##F G
	classroom$$ 
.$$ 
Id$$ 
,$$ 
	classroom$$ #
.$$# $
Name$$$ (
,$$( )
	classroom$$* 3
.$$3 4
Description$$4 ?
,$$? @
	classroom%% 
.%% 
SubjectType%% !
,%%! "
	classroom%%# ,
.%%, -
	TeacherId%%- 6
,%%6 7
	classroom%%8 A
.%%A B
	ClassCode%%B K
,%%K L
	classroom&& 
.&& 
MaxStudents&& !
,&&! "
	classroom&&# ,
.&&, -
IsActive&&- 5
,&&5 6
memberCount&&7 B
,&&B C
lessonCount&&D O
)'' 	
)''	 

;''
 
}(( 
})) ƒ
yD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\UpdateGrade\UpdateGradeCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Commands2 :
.: ;
UpdateGrade; F
;F G
public 
record 
UpdateGradeCommand  
(  !
Guid 
GradeId	 
, 
Guid		 
	TeacherId			 
,		 
int

 
Value

 
,

 
string 

Description 
) 
: 
IRequest 
< 
Result 
< 
GradeResponse !
>! "
>" #
;# $‹
ÄD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Commands\UpdateGrade\UpdateGradeCommandHandler.cs
	namespace		 	
AiTutor		
 
.		 
Application		 
.		 
Features		 &
.		& '

Classrooms		' 1
.		1 2
Commands		2 :
.		: ;
UpdateGrade		; F
;		F G
public 
class %
UpdateGradeCommandHandler &
:' (
IRequestHandler) 8
<8 9
UpdateGradeCommand9 K
,K L
ResultM S
<S T
GradeResponseT a
>a b
>b c
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
%
UpdateGradeCommandHandler $
($ %!
IApplicationDbContext% :
context; B
)B C
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
GradeResponse *
>* +
>+ ,
Handle- 3
(3 4
UpdateGradeCommand4 F
requestG N
,N O
CancellationTokenP a
ctb d
)d e
{ 
var 
grade 
= 
await 
_context "
." #
Grades# )
. 
Include 
( 
g 
=> 
g 
. 
Student #
)# $
. 
FirstOrDefaultAsync  
(  !
g! "
=># %
g& '
.' (
Id( *
==+ -
request. 5
.5 6
GradeId6 =
,= >
ct? A
)A B
;B C
if 

( 
grade 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
Grade/ 4
)4 5
,5 6
request7 >
.> ?
GradeId? F
)F G
;G H
if 

( 
grade 
. 
	TeacherId 
!= 
request &
.& '
	TeacherId' 0
)0 1
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
grade 
. 
Update 
( 
request 
. 
Value "
," #
request$ +
.+ ,
Description, 7
)7 8
;8 9
await 
_context 
. 
SaveChangesAsync '
(' (
ct( *
)* +
;+ ,
return 
Result 
< 
GradeResponse #
># $
.$ %
Success% ,
(, -
new- 0
GradeResponse1 >
(> ?
grade   
.   
Id   
,   
grade   
.   
ClassroomId   '
,  ' (
grade  ) .
.  . /
	StudentId  / 8
,  8 9
grade!! 
.!! 
Student!! 
.!! 
FullName!! "
,!!" #
grade!!$ )
.!!) *
	TeacherId!!* 3
,!!3 4
grade"" 
."" 
Value"" 
,"" 
grade"" 
."" 
Description"" *
,""* +
grade"", 1
.""1 2
GradedAt""2 :
)## 	
)##	 

;##
 
}$$ 
}%% Î
ÄD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomById\GetClassroomByIdQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Queries2 9
.9 :
GetClassroomById: J
;J K
public 
record !
GetClassroomByIdQuery #
(# $
Guid$ (
ClassroomId) 4
,4 5
Guid6 :
UserId; A
)A B
:C D
IRequestE M
<M N
ResultN T
<T U
ClassroomResponseU f
>f g
>g h
;h iÒ'
áD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomById\GetClassroomByIdQueryHandler.cs
	namespace		 	
AiTutor		
 
.		 
Application		 
.		 
Features		 &
.		& '

Classrooms		' 1
.		1 2
Queries		2 9
.		9 :
GetClassroomById		: J
;		J K
public 
class (
GetClassroomByIdQueryHandler )
: 
IRequestHandler 
< !
GetClassroomByIdQuery +
,+ ,
Result- 3
<3 4
ClassroomResponse4 E
>E F
>F G
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
(
GetClassroomByIdQueryHandler '
(' (!
IApplicationDbContext( =
context> E
)E F
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
ClassroomResponse .
>. /
>/ 0
Handle1 7
(7 8!
GetClassroomByIdQuery 
request %
,% &
CancellationToken' 8
ct9 ;
); <
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
var 
	isTeacher 
= 
	classroom !
.! "
	TeacherId" +
==, .
request/ 6
.6 7
UserId7 =
;= >
var 
	isStudent 
= 
await 
_context &
.& '
ClassroomMembers' 7
. 
AnyAsync 
( 
m 
=> 
m 
. 
ClassroomId (
==) +
request, 3
.3 4
ClassroomId4 ?
&&@ B
m 
. 
	StudentId %
==& (
request) 0
.0 1
UserId1 7
&&8 :
m; <
.< =
IsActive= E
,E F
ctG I
)I J
;J K
if   

(   
!   
	isTeacher   
&&   
!   
	isStudent   $
)  $ %
throw!! 
new!! $
ForbiddenAccessException!! .
(!!. /
)!!/ 0
;!!0 1
var## 
memberCount## 
=## 
await## 
_context##  (
.##( )
ClassroomMembers##) 9
.$$ 

CountAsync$$ 
($$ 
m$$ 
=>$$ 
m$$ 
.$$ 
ClassroomId$$ *
==$$+ -
	classroom$$. 7
.$$7 8
Id$$8 :
&&$$; =
m$$> ?
.$$? @
IsActive$$@ H
,$$H I
ct$$J L
)$$L M
;$$M N
var%% 
lessonCount%% 
=%% 
await%% 
_context%%  (
.%%( )
ClassroomLessons%%) 9
.&& 

CountAsync&& 
(&& 
l&& 
=>&& 
l&& 
.&& 
ClassroomId&& *
==&&+ -
	classroom&&. 7
.&&7 8
Id&&8 :
,&&: ;
ct&&< >
)&&> ?
;&&? @
return(( 
Result(( 
<(( 
ClassroomResponse(( '
>((' (
.((( )
Success(() 0
(((0 1
new((1 4
ClassroomResponse((5 F
(((F G
	classroom)) 
.)) 
Id)) 
,)) 
	classroom)) #
.))# $
Name))$ (
,))( )
	classroom))* 3
.))3 4
Description))4 ?
,))? @
	classroom** 
.** 
SubjectType** !
,**! "
	classroom**# ,
.**, -
	TeacherId**- 6
,**6 7
	classroom**8 A
.**A B
	ClassCode**B K
,**K L
	classroom++ 
.++ 
MaxStudents++ !
,++! "
	classroom++# ,
.++, -
IsActive++- 5
,++5 6
memberCount++7 B
,++B C
lessonCount++D O
),, 	
),,	 

;,,
 
}-- 
}.. ◊
ÑD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomGrades\GetClassroomGradesQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Queries2 9
.9 :
GetClassroomGrades: L
;L M
public 
record #
GetClassroomGradesQuery %
(% &
Guid& *
ClassroomId+ 6
,6 7
Guid8 <
UserId= C
,C D
boolE I
	IsTeacherJ S
)S T
: 
IRequest 
< 
Result 
< 
List 
< 
GradeResponse (
>( )
>) *
>* +
;+ ,ˇ$
ãD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomGrades\GetClassroomGradesQueryHandler.cs
	namespace		 	
AiTutor		
 
.		 
Application		 
.		 
Features		 &
.		& '

Classrooms		' 1
.		1 2
Queries		2 9
.		9 :
GetClassroomGrades		: L
;		L M
public 
class *
GetClassroomGradesQueryHandler +
: 
IRequestHandler 
< #
GetClassroomGradesQuery -
,- .
Result/ 5
<5 6
List6 :
<: ;
GradeResponse; H
>H I
>I J
>J K
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
*
GetClassroomGradesQueryHandler )
() *!
IApplicationDbContext* ?
context@ G
)G H
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
List !
<! "
GradeResponse" /
>/ 0
>0 1
>1 2
Handle3 9
(9 :#
GetClassroomGradesQuery 
request  '
,' (
CancellationToken) :
ct; =
)= >
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P

IQueryable 
< 
Grade 
> 
query 
=  !
_context" *
.* +
Grades+ 1
. 
Include 
( 
g 
=> 
g 
. 
Student #
)# $
. 
Where 
( 
g 
=> 
g 
. 
ClassroomId %
==& (
request) 0
.0 1
ClassroomId1 <
)< =
;= >
if 

( 
! 
request 
. 
	IsTeacher 
) 
query   
=   
query   
.   
Where   
(    
g    !
=>  " $
g  % &
.  & '
	StudentId  ' 0
==  1 3
request  4 ;
.  ; <
UserId  < B
)  B C
;  C D
var"" 
grades"" 
="" 
await"" 
query""  
.## 
OrderByDescending## 
(## 
g##  
=>##! #
g##$ %
.##% &
GradedAt##& .
)##. /
.$$ 
ToListAsync$$ 
($$ 
ct$$ 
)$$ 
;$$ 
var&& 
result&& 
=&& 
grades&& 
.&& 
Select&& "
(&&" #
g&&# $
=>&&% '
new&&( +
GradeResponse&&, 9
(&&9 :
g'' 
.'' 
Id'' 
,'' 
g'' 
.'' 
ClassroomId'' 
,''  
g''! "
.''" #
	StudentId''# ,
,'', -
g(( 
.(( 
Student(( 
.(( 
FullName(( 
,(( 
g((  !
.((! "
	TeacherId((" +
,((+ ,
g)) 
.)) 
Value)) 
,)) 
g)) 
.)) 
Description)) "
,))" #
g))$ %
.))% &
GradedAt))& .
)** 	
)**	 

.**
 
ToList** 
(** 
)** 
;** 
return,, 
Result,, 
<,, 
List,, 
<,, 
GradeResponse,, (
>,,( )
>,,) *
.,,* +
Success,,+ 2
(,,2 3
result,,3 9
),,9 :
;,,: ;
}-- 
}.. æ
åD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomLessonById\GetClassroomLessonByIdQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Queries2 9
.9 :"
GetClassroomLessonById: P
;P Q
public 
record '
GetClassroomLessonByIdQuery )
() *
Guid 
ClassroomId	 
, 
Guid		 
LessonId			 
,		 
Guid

 
UserId

	 
) 
: 
IRequest 
< 
Result 
< #
ClassroomLessonResponse +
>+ ,
>, -
;- .ù'
ìD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomLessonById\GetClassroomLessonByIdQueryHandler.cs
	namespace		 	
AiTutor		
 
.		 
Application		 
.		 
Features		 &
.		& '

Classrooms		' 1
.		1 2
Queries		2 9
.		9 :"
GetClassroomLessonById		: P
;		P Q
public 
class .
"GetClassroomLessonByIdQueryHandler /
: 
IRequestHandler 
< '
GetClassroomLessonByIdQuery 1
,1 2
Result3 9
<9 :#
ClassroomLessonResponse: Q
>Q R
>R S
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
.
"GetClassroomLessonByIdQueryHandler -
(- .!
IApplicationDbContext. C
contextD K
)K L
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< #
ClassroomLessonResponse 4
>4 5
>5 6
Handle7 =
(= >'
GetClassroomLessonByIdQuery #
request$ +
,+ ,
CancellationToken- >
ct? A
)A B
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
var 
	isTeacher 
= 
	classroom !
.! "
	TeacherId" +
==, .
request/ 6
.6 7
UserId7 =
;= >
var 
	isStudent 
= 
await 
_context &
.& '
ClassroomMembers' 7
. 
AnyAsync 
( 
m 
=> 
m 
. 
ClassroomId (
==) +
request, 3
.3 4
ClassroomId4 ?
&&@ B
m 
. 
	StudentId %
==& (
request) 0
.0 1
UserId1 7
&&8 :
m; <
.< =
IsActive= E
,E F
ctG I
)I J
;J K
if 

( 
! 
	isTeacher 
&& 
! 
	isStudent $
)$ %
throw   
new   $
ForbiddenAccessException   .
(  . /
)  / 0
;  0 1
var"" 
lesson"" 
="" 
await"" 
_context"" #
.""# $
ClassroomLessons""$ 4
.## 
FirstOrDefaultAsync##  
(##  !
l##! "
=>### %
l##& '
.##' (
Id##( *
==##+ -
request##. 5
.##5 6
LessonId##6 >
&&##? A
l$$& '
.$$' (
ClassroomId$$( 3
==$$4 6
request$$7 >
.$$> ?
ClassroomId$$? J
,$$J K
ct$$L N
)$$N O
;$$O P
if%% 

(%% 
lesson%% 
is%% 
null%% 
)%% 
throw&& 
new&& 
NotFoundException&& '
(&&' (
nameof&&( .
(&&. /
ClassroomLesson&&/ >
)&&> ?
,&&? @
request&&A H
.&&H I
LessonId&&I Q
)&&Q R
;&&R S
return(( 
Result(( 
<(( #
ClassroomLessonResponse(( -
>((- .
.((. /
Success((/ 6
(((6 7
new((7 :#
ClassroomLessonResponse((; R
(((R S
lesson)) 
.)) 
Id)) 
,)) 
lesson)) 
.)) 
ClassroomId)) )
,))) *
lesson))+ 1
.))1 2
Title))2 7
,))7 8
lesson))9 ?
.))? @
Content))@ G
,))G H
lesson** 
.** 

OrderIndex** 
,** 
lesson** %
.**% &

Difficulty**& 0
,**0 1
lesson**2 8
.**8 9
	CreatedAt**9 B
)++ 	
)++	 

;++
 
},, 
}-- ≠
ÜD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomLessons\GetClassroomLessonsQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Queries2 9
.9 :
GetClassroomLessons: M
;M N
public 
record $
GetClassroomLessonsQuery &
(& '
Guid' +
ClassroomId, 7
,7 8
Guid9 =
UserId> D
)D E
: 
IRequest 
< 
Result 
< 
List 
< #
ClassroomLessonResponse 2
>2 3
>3 4
>4 5
;5 6œ'
çD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomLessons\GetClassroomLessonsQueryHandler.cs
	namespace		 	
AiTutor		
 
.		 
Application		 
.		 
Features		 &
.		& '

Classrooms		' 1
.		1 2
Queries		2 9
.		9 :
GetClassroomLessons		: M
;		M N
public 
class +
GetClassroomLessonsQueryHandler ,
: 
IRequestHandler 
< $
GetClassroomLessonsQuery .
,. /
Result0 6
<6 7
List7 ;
<; <#
ClassroomLessonResponse< S
>S T
>T U
>U V
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
+
GetClassroomLessonsQueryHandler *
(* +!
IApplicationDbContext+ @
contextA H
)H I
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
List !
<! "#
ClassroomLessonResponse" 9
>9 :
>: ;
>; <
Handle= C
(C D$
GetClassroomLessonsQuery  
request! (
,( )
CancellationToken* ;
ct< >
)> ?
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
var 
	isTeacher 
= 
	classroom !
.! "
	TeacherId" +
==, .
request/ 6
.6 7
UserId7 =
;= >
var 
	isStudent 
= 
await 
_context &
.& '
ClassroomMembers' 7
. 
AnyAsync 
( 
m 
=> 
m 
. 
ClassroomId (
==) +
request, 3
.3 4
ClassroomId4 ?
&&@ B
m 
. 
	StudentId %
==& (
request) 0
.0 1
UserId1 7
&&8 :
m; <
.< =
IsActive= E
,E F
ctG I
)I J
;J K
if 

( 
! 
	isTeacher 
&& 
! 
	isStudent $
)$ %
throw   
new   $
ForbiddenAccessException   .
(  . /
)  / 0
;  0 1
var"" 
lessons"" 
="" 
await"" 
_context"" $
.""$ %
ClassroomLessons""% 5
.## 
Where## 
(## 
l## 
=>## 
l## 
.## 
ClassroomId## %
==##& (
request##) 0
.##0 1
ClassroomId##1 <
)##< =
.$$ 
OrderBy$$ 
($$ 
l$$ 
=>$$ 
l$$ 
.$$ 

OrderIndex$$ &
)$$& '
.%% 
ToListAsync%% 
(%% 
ct%% 
)%% 
;%% 
var'' 
result'' 
='' 
lessons'' 
.'' 
Select'' #
(''# $
l''$ %
=>''& (
new'') ,#
ClassroomLessonResponse''- D
(''D E
l(( 
.(( 
Id(( 
,(( 
l(( 
.(( 
ClassroomId(( 
,((  
l((! "
.((" #
Title((# (
,((( )
l((* +
.((+ ,
Content((, 3
,((3 4
l)) 
.)) 

OrderIndex)) 
,)) 
l)) 
.)) 

Difficulty)) &
,))& '
l))( )
.))) *
	CreatedAt))* 3
)** 	
)**	 

.**
 
ToList** 
(** 
)** 
;** 
return,, 
Result,, 
<,, 
List,, 
<,, #
ClassroomLessonResponse,, 2
>,,2 3
>,,3 4
.,,4 5
Success,,5 <
(,,< =
result,,= C
),,C D
;,,D E
}-- 
}.. Ô
ÜD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomMembers\GetClassroomMembersQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Queries2 9
.9 :
GetClassroomMembers: M
;M N
public 
record $
GetClassroomMembersQuery &
(& '
Guid' +
ClassroomId, 7
,7 8
Guid9 =
	TeacherId> G
)G H
: 
IRequest 
< 
Result 
< 
List 
< 
ClassroomMemberDto -
>- .
>. /
>/ 0
;0 1
public		 
record		 
ClassroomMemberDto		  
(		  !
Guid

 
	StudentId

	 
,

 
string 

FullName 
, 
string 

Email 
, 
DateTime 
JoinedAt 
) 
; °"
çD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomMembers\GetClassroomMembersQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Queries2 9
.9 :
GetClassroomMembers: M
;M N
public

 
class

 +
GetClassroomMembersQueryHandler

 ,
: 
IRequestHandler 
< $
GetClassroomMembersQuery .
,. /
Result0 6
<6 7
List7 ;
<; <
ClassroomMemberDto< N
>N O
>O P
>P Q
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
+
GetClassroomMembersQueryHandler *
(* +!
IApplicationDbContext+ @
contextA H
)H I
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
List !
<! "
ClassroomMemberDto" 4
>4 5
>5 6
>6 7
Handle8 >
(> ?$
GetClassroomMembersQuery  
request! (
,( )
CancellationToken* ;
ct< >
)> ?
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
if 

( 
	classroom 
. 
	TeacherId 
!=  "
request# *
.* +
	TeacherId+ 4
)4 5
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
var 
members 
= 
await 
_context $
.$ %
ClassroomMembers% 5
. 
Include 
( 
m 
=> 
m 
. 
Student #
)# $
. 
Where 
( 
m 
=> 
m 
. 
ClassroomId %
==& (
request) 0
.0 1
ClassroomId1 <
&&= ?
m@ A
.A B
IsActiveB J
)J K
. 
OrderBy 
( 
m 
=> 
m 
. 
JoinedAt $
)$ %
.   
ToListAsync   
(   
ct   
)   
;   
var"" 
result"" 
="" 
members"" 
."" 
Select"" #
(""# $
m""$ %
=>""& (
new"") ,
ClassroomMemberDto""- ?
(""? @
m## 
.## 
	StudentId## 
,## 
m$$ 
.$$ 
Student$$ 
.$$ 
FullName$$ 
,$$ 
m%% 
.%% 
Student%% 
.%% 
Email%% 
.%% 
Value%% !
,%%! "
m&& 
.&& 
JoinedAt&& 
)'' 	
)''	 

.''
 
ToList'' 
('' 
)'' 
;'' 
return)) 
Result)) 
<)) 
List)) 
<)) 
ClassroomMemberDto)) -
>))- .
>)). /
.))/ 0
Success))0 7
())7 8
result))8 >
)))> ?
;))? @
}** 
}++ é
àD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomProgress\GetClassroomProgressQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Queries2 9
.9 : 
GetClassroomProgress: N
;N O
public 
record %
ClassroomProgressResponse '
(' (
Guid 
	StudentId	 
, 
string 

StudentName 
, 
Guid		 
ClassroomLessonId			 
,		 
string

 

LessonTitle

 
,

 
bool 
IsCompleted	 
, 
double 

ScorePercentage 
, 
int 
AttemptsCount 
, 
DateTime 
? 
CompletedAt 
) 
; 
public 
record %
GetClassroomProgressQuery '
(' (
Guid( ,
ClassroomId- 8
,8 9
Guid: >
UserId? E
)E F
:G H
IRequestI Q
<Q R
ResultR X
<X Y
ListY ]
<] ^%
ClassroomProgressResponse^ w
>w x
>x y
>y z
;z {Î:
èD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomProgress\GetClassroomProgressQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Queries2 9
.9 : 
GetClassroomProgress: N
;N O
public		 
class		 ,
 GetClassroomProgressQueryHandler		 -
:		. /
IRequestHandler		0 ?
<		? @%
GetClassroomProgressQuery		@ Y
,		Y Z
Result		[ a
<		a b
List		b f
<		f g&
ClassroomProgressResponse			g Ä
>
		Ä Å
>
		Å Ç
>
		Ç É
{

 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
,
 GetClassroomProgressQueryHandler +
(+ ,!
IApplicationDbContext, A
contextB I
)I J
=>K M
_contextN V
=W X
contextY `
;` a
public 

async 
Task 
< 
Result 
< 
List !
<! "%
ClassroomProgressResponse" ;
>; <
>< =
>= >
Handle? E
(E F%
GetClassroomProgressQueryF _
request` g
,g h
CancellationTokeni z
ct{ }
)} ~
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
?? 
throw 
new 
NotFoundException *
(* +
$str+ 6
,6 7
request8 ?
.? @
ClassroomId@ K
)K L
;L M
var 
	isTeacher 
= 
	classroom !
.! "
	TeacherId" +
==, .
request/ 6
.6 7
UserId7 =
;= >
var 
isMember 
= 
await 
_context %
.% &
ClassroomMembers& 6
. 
AnyAsync 
( 
m 
=> 
m 
. 
ClassroomId (
==) +
request, 3
.3 4
ClassroomId4 ?
&&@ B
mC D
.D E
	StudentIdE N
==O Q
requestR Y
.Y Z
UserIdZ `
&&a c
md e
.e f
IsActivef n
,n o
ctp r
)r s
;s t
if 

( 
! 
	isTeacher 
&& 
! 
isMember #
)# $
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
var 
progressQuery 
= 
_context $
.$ %
ClassroomProgresses% 8
. 
Where 
( 
p 
=> 
p 
. 
ClassroomId %
==& (
request) 0
.0 1
ClassroomId1 <
)< =
;= >
if 

( 
! 
	isTeacher 
) 
progressQuery 
= 
progressQuery )
.) *
Where* /
(/ 0
p0 1
=>2 4
p5 6
.6 7
	StudentId7 @
==A C
requestD K
.K L
UserIdL R
)R S
;S T
var 

progresses 
= 
await 
progressQuery ,
., -
ToListAsync- 8
(8 9
ct9 ;
); <
;< =
var 
lessons 
= 
await 
_context $
.$ %
ClassroomLessons% 5
. 
Where 
( 
l 
=> 
l 
. 
ClassroomId %
==& (
request) 0
.0 1
ClassroomId1 <
)< =
. 
ToDictionaryAsync 
( 
l  
=>! #
l$ %
.% &
Id& (
,( )
l* +
=>, .
l/ 0
.0 1
Title1 6
,6 7
ct8 :
): ;
;; <
var 

studentIds 
= 

progresses #
.# $
Select$ *
(* +
p+ ,
=>- /
p0 1
.1 2
	StudentId2 ;
); <
.< =
Distinct= E
(E F
)F G
.G H
ToListH N
(N O
)O P
;P Q
var   
users   
=   
await   
_context   "
.  " #
Users  # (
.!! 
Where!! 
(!! 
u!! 
=>!! 

studentIds!! "
.!!" #
Contains!!# +
(!!+ ,
u!!, -
.!!- .
Id!!. 0
)!!0 1
)!!1 2
."" 
ToDictionaryAsync"" 
("" 
u""  
=>""! #
u""$ %
.""% &
Id""& (
,""( )
u""* +
=>"", .
u""/ 0
.""0 1
FullName""1 9
,""9 :
ct""; =
)""= >
;""> ?
var## 
response## 
=## 

progresses## !
.##! "
Select##" (
(##( )
p##) *
=>##+ -
new##. 1%
ClassroomProgressResponse##2 K
(##K L
p$$ 
.$$ 
	StudentId$$ 
,$$ 
users%% 
.%% 
TryGetValue%% 
(%% 
p%% 
.%%  
	StudentId%%  )
,%%) *
out%%+ .
var%%/ 2
name%%3 7
)%%7 8
?%%9 :
name%%; ?
:%%@ A
$str%%B K
,%%K L
p&& 
.&& 
ClassroomLessonId&& 
,&&  
lessons'' 
.'' 
TryGetValue'' 
(''  
p''  !
.''! "
ClassroomLessonId''" 3
,''3 4
out''5 8
var''9 <
title''= B
)''B C
?''D E
title''F K
:''L M
$str''N W
,''W X
p(( 
.(( 
IsCompleted(( 
,(( 
p(( 
.(( 
ScorePercentage(( ,
,((, -
p((. /
.((/ 0
AttemptsCount((0 =
,((= >
p((? @
.((@ A
CompletedAt((A L
)((L M
)((M N
.((N O
ToList((O U
(((U V
)((V W
;((W X
return)) 
Result)) 
<)) 
List)) 
<)) %
ClassroomProgressResponse)) 4
>))4 5
>))5 6
.))6 7
Success))7 >
())> ?
response))? G
)))G H
;))H I
}** 
}++ ø
àD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomQuizById\GetClassroomQuizByIdQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Queries2 9
.9 : 
GetClassroomQuizById: N
;N O
public 
record %
GetClassroomQuizByIdQuery '
(' (
Guid 
ClassroomId	 
, 
Guid		 
QuizId			 
,		 
Guid

 
UserId

	 
) 
: 
IRequest 
< 
Result 
< '
ClassroomQuizDetailResponse /
>/ 0
>0 1
;1 2
public 
record '
ClassroomQuizDetailResponse )
() *
Guid 
Id	 
, 
Guid 
ClassroomLessonId	 
, 
Guid 
ClassroomId	 
, 
string 

Title 
, 
DifficultyLevel 

Difficulty 
, 
int 
TimeLimitMinutes 
, 
DateTime 
	CreatedAt 
, 
List 
< 	+
ClassroomQuestionDetailResponse	 (
>( )
	Questions* 3
) 
; 
public 
record +
ClassroomQuestionDetailResponse -
(- .
Guid 
Id	 
, 
string 

Text 
, 
string 

CorrectAnswer 
, 
List 
< 	
string	 
> 
Options 
, 
int 
Points 
, 
string 

?
 
Explanation 
) 
; Ã.
èD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomQuizById\GetClassroomQuizByIdQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Queries2 9
.9 : 
GetClassroomQuizById: N
;N O
public

 
class

 ,
 GetClassroomQuizByIdQueryHandler

 -
: 
IRequestHandler 
< %
GetClassroomQuizByIdQuery /
,/ 0
Result1 7
<7 8'
ClassroomQuizDetailResponse8 S
>S T
>T U
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
,
 GetClassroomQuizByIdQueryHandler +
(+ ,!
IApplicationDbContext, A
contextB I
)I J
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< '
ClassroomQuizDetailResponse 8
>8 9
>9 :
Handle; A
(A B%
GetClassroomQuizByIdQuery !
request" )
,) *
CancellationToken+ <
ct= ?
)? @
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
var 
	isTeacher 
= 
	classroom !
.! "
	TeacherId" +
==, .
request/ 6
.6 7
UserId7 =
;= >
var 
	isStudent 
= 
await 
_context &
.& '
ClassroomMembers' 7
. 
AnyAsync 
( 
m 
=> 
m 
. 
ClassroomId (
==) +
request, 3
.3 4
ClassroomId4 ?
&&@ B
m 
. 
	StudentId %
==& (
request) 0
.0 1
UserId1 7
&&8 :
m; <
.< =
IsActive= E
,E F
ctG I
)I J
;J K
if 

( 
! 
	isTeacher 
&& 
! 
	isStudent $
)$ %
throw 
new $
ForbiddenAccessException .
(. /
)/ 0
;0 1
var!! 
quiz!! 
=!! 
await!! 
_context!! !
.!!! "
ClassroomQuizzes!!" 2
."" 
Include"" 
("" 
q"" 
=>"" 
q"" 
."" 
	Questions"" %
)""% &
.## 
FirstOrDefaultAsync##  
(##  !
q##! "
=>### %
q##& '
.##' (
Id##( *
==##+ -
request##. 5
.##5 6
QuizId##6 <
&&##= ?
q$$& '
.$$' (
ClassroomId$$( 3
==$$4 6
request$$7 >
.$$> ?
ClassroomId$$? J
,$$J K
ct$$L N
)$$N O
;$$O P
if%% 

(%% 
quiz%% 
is%% 
null%% 
)%% 
throw&& 
new&& 
NotFoundException&& '
(&&' (
nameof&&( .
(&&. /
ClassroomQuiz&&/ <
)&&< =
,&&= >
request&&? F
.&&F G
QuizId&&G M
)&&M N
;&&N O
var(( 
	questions(( 
=(( 
quiz(( 
.(( 
	Questions(( &
.((& '
Select((' -
(((- .
q((. /
=>((0 2
new((3 6+
ClassroomQuestionDetailResponse((7 V
(((V W
q)) 
.)) 
Id)) 
,)) 
q)) 
.)) 
Text)) 
,)) 
q)) 
.)) 
CorrectAnswer)) )
,))) *
q))+ ,
.)), -
Options))- 4
,))4 5
q))6 7
.))7 8
Points))8 >
,))> ?
q))@ A
.))A B
Explanation))B M
)** 	
)**	 

.**
 
ToList** 
(** 
)** 
;** 
return,, 
Result,, 
<,, '
ClassroomQuizDetailResponse,, 1
>,,1 2
.,,2 3
Success,,3 :
(,,: ;
new,,; >'
ClassroomQuizDetailResponse,,? Z
(,,Z [
quiz-- 
.-- 
Id-- 
,-- 
quiz-- 
.-- 
ClassroomLessonId-- +
,--+ ,
quiz--- 1
.--1 2
ClassroomId--2 =
,--= >
quiz--? C
.--C D
Title--D I
,--I J
quiz.. 
... 

Difficulty.. 
,.. 
quiz.. !
...! "
TimeLimitMinutes.." 2
,..2 3
quiz..4 8
...8 9
	CreatedAt..9 B
,..B C
	questions..D M
)// 	
)//	 

;//
 
}00 
}11 Ä
ñD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomQuizzesByLesson\GetClassroomQuizzesByLessonQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Queries2 9
.9 :'
GetClassroomQuizzesByLesson: U
;U V
public 
record ,
 GetClassroomQuizzesByLessonQuery .
(. /
Guid 
ClassroomId	 
, 
Guid		 
LessonId			 
,		 
Guid

 
UserId

	 
) 
: 
IRequest 
< 
Result 
< 
List 
< !
ClassroomQuizResponse .
>. /
>/ 0
>0 1
;1 2°+
ùD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetClassroomQuizzesByLesson\GetClassroomQuizzesByLessonQueryHandler.cs
	namespace		 	
AiTutor		
 
.		 
Application		 
.		 
Features		 &
.		& '

Classrooms		' 1
.		1 2
Queries		2 9
.		9 :'
GetClassroomQuizzesByLesson		: U
;		U V
public 
class 3
'GetClassroomQuizzesByLessonQueryHandler 4
: 
IRequestHandler 
< ,
 GetClassroomQuizzesByLessonQuery 6
,6 7
Result8 >
<> ?
List? C
<C D!
ClassroomQuizResponseD Y
>Y Z
>Z [
>[ \
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
3
'GetClassroomQuizzesByLessonQueryHandler 2
(2 3!
IApplicationDbContext3 H
contextI P
)P Q
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
List !
<! "!
ClassroomQuizResponse" 7
>7 8
>8 9
>9 :
Handle; A
(A B,
 GetClassroomQuizzesByLessonQuery (
request) 0
,0 1
CancellationToken2 C
ctD F
)F G
{ 
var 
	classroom 
= 
await 
_context &
.& '

Classrooms' 1
. 
FirstOrDefaultAsync  
(  !
c! "
=># %
c& '
.' (
Id( *
==+ -
request. 5
.5 6
ClassroomId6 A
,A B
ctC E
)E F
;F G
if 

( 
	classroom 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
	Classroom/ 8
)8 9
,9 :
request; B
.B C
ClassroomIdC N
)N O
;O P
var 
	isTeacher 
= 
	classroom !
.! "
	TeacherId" +
==, .
request/ 6
.6 7
UserId7 =
;= >
var 
	isStudent 
= 
await 
_context &
.& '
ClassroomMembers' 7
. 
AnyAsync 
( 
m 
=> 
m 
. 
ClassroomId (
==) +
request, 3
.3 4
ClassroomId4 ?
&&@ B
m 
. 
	StudentId %
==& (
request) 0
.0 1
UserId1 7
&&8 :
m; <
.< =
IsActive= E
,E F
ctG I
)I J
;J K
if 

( 
! 
	isTeacher 
&& 
! 
	isStudent $
)$ %
throw   
new   $
ForbiddenAccessException   .
(  . /
)  / 0
;  0 1
var"" 
quizzes"" 
="" 
await"" 
_context"" $
.""$ %
ClassroomQuizzes""% 5
.## 
Include## 
(## 
q## 
=>## 
q## 
.## 
	Questions## %
)##% &
.$$ 
Where$$ 
($$ 
q$$ 
=>$$ 
q$$ 
.$$ 
ClassroomLessonId$$ +
==$$, .
request$$/ 6
.$$6 7
LessonId$$7 ?
&&$$@ B
q%% 
.%% 
ClassroomId%% %
==%%& (
request%%) 0
.%%0 1
ClassroomId%%1 <
)%%< =
.&& 
OrderBy&& 
(&& 
q&& 
=>&& 
q&& 
.&& 
	CreatedAt&& %
)&&% &
.'' 
ToListAsync'' 
('' 
ct'' 
)'' 
;'' 
var)) 
result)) 
=)) 
quizzes)) 
.)) 
Select)) #
())# $
q))$ %
=>))& (
new))) ,!
ClassroomQuizResponse))- B
())B C
q** 
.** 
Id** 
,** 
q** 
.** 
ClassroomLessonId** %
,**% &
q**' (
.**( )
ClassroomId**) 4
,**4 5
q**6 7
.**7 8
Title**8 =
,**= >
q++ 
.++ 

Difficulty++ 
,++ 
q++ 
.++ 
TimeLimitMinutes++ ,
,++, -
q++. /
.++/ 0
	Questions++0 9
.++9 :
Count++: ?
,++? @
q++A B
.++B C
	CreatedAt++C L
),, 	
),,	 

.,,
 
ToList,, 
(,, 
),, 
;,, 
return.. 
Result.. 
<.. 
List.. 
<.. !
ClassroomQuizResponse.. 0
>..0 1
>..1 2
...2 3
Success..3 :
(..: ;
result..; A
)..A B
;..B C
}// 
}00 ‹
~D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetMyClassrooms\GetMyClassroomsQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Queries2 9
.9 :
GetMyClassrooms: I
;I J
public 
record  
GetMyClassroomsQuery "
(" #
Guid# '
UserId( .
). /
:0 1
IRequest2 :
<: ;
Result; A
<A B
ListB F
<F G
ClassroomResponseG X
>X Y
>Y Z
>Z [
;[ \™+
ÖD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Classrooms\Queries\GetMyClassrooms\GetMyClassroomsQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '

Classrooms' 1
.1 2
Queries2 9
.9 :
GetMyClassrooms: I
;I J
public		 
class		 '
GetMyClassroomsQueryHandler		 (
:

 
IRequestHandler

 
<

  
GetMyClassroomsQuery

 *
,

* +
Result

, 2
<

2 3
List

3 7
<

7 8
ClassroomResponse

8 I
>

I J
>

J K
>

K L
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
'
GetMyClassroomsQueryHandler &
(& '!
IApplicationDbContext' <
context= D
)D E
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
List !
<! "
ClassroomResponse" 3
>3 4
>4 5
>5 6
Handle7 =
(= > 
GetMyClassroomsQuery 
request $
,$ %
CancellationToken& 7
ct8 :
): ;
{ 
var 
	asTeacher 
= 
await 
_context &
.& '

Classrooms' 1
. 
Where 
( 
c 
=> 
c 
. 
	TeacherId #
==$ &
request' .
.. /
UserId/ 5
&&6 8
c9 :
.: ;
IsActive; C
)C D
. 
ToListAsync 
( 
ct 
) 
; 
var 
	asStudent 
= 
await 
_context &
.& '
ClassroomMembers' 7
. 
Include 
( 
m 
=> 
m 
. 
	Classroom %
)% &
. 
Where 
( 
m 
=> 
m 
. 
	StudentId #
==$ &
request' .
.. /
UserId/ 5
&&6 8
m9 :
.: ;
IsActive; C
&&D F
mG H
.H I
	ClassroomI R
.R S
IsActiveS [
)[ \
. 
Select 
( 
m 
=> 
m 
. 
	Classroom $
)$ %
. 
ToListAsync 
( 
ct 
) 
; 
var   
allClassrooms   
=   
	asTeacher   %
.  % &
Union  & +
(  + ,
	asStudent  , 5
)  5 6
.  6 7
Distinct  7 ?
(  ? @
)  @ A
.  A B
ToList  B H
(  H I
)  I J
;  J K
var"" 
response"" 
="" 
new"" 
List"" 
<""  
ClassroomResponse""  1
>""1 2
(""2 3
)""3 4
;""4 5
foreach## 
(## 
var## 
c## 
in## 
allClassrooms## '
)##' (
{$$ 	
var%% 
memberCount%% 
=%% 
await%% #
_context%%$ ,
.%%, -
ClassroomMembers%%- =
.&& 

CountAsync&& 
(&& 
m&& 
=>&&  
m&&! "
.&&" #
ClassroomId&&# .
==&&/ 1
c&&2 3
.&&3 4
Id&&4 6
&&&&7 9
m&&: ;
.&&; <
IsActive&&< D
,&&D E
ct&&F H
)&&H I
;&&I J
var'' 
lessonCount'' 
='' 
await'' #
_context''$ ,
.'', -
ClassroomLessons''- =
.(( 

CountAsync(( 
((( 
l(( 
=>((  
l((! "
.((" #
ClassroomId((# .
==((/ 1
c((2 3
.((3 4
Id((4 6
,((6 7
ct((8 :
)((: ;
;((; <
response)) 
.)) 
Add)) 
()) 
new)) 
ClassroomResponse)) .
()). /
c** 
.** 
Id** 
,** 
c** 
.** 
Name** 
,** 
c** 
.**  
Description**  +
,**+ ,
c**- .
.**. /
SubjectType**/ :
,**: ;
c++ 
.++ 
	TeacherId++ 
,++ 
c++ 
.++ 
	ClassCode++ (
,++( )
c++* +
.+++ ,
MaxStudents++, 7
,++7 8
c,, 
.,, 
IsActive,, 
,,, 
memberCount,, '
,,,' (
lessonCount,,) 4
)-- 
)-- 
;-- 
}.. 	
return00 
Result00 
<00 
List00 
<00 
ClassroomResponse00 ,
>00, -
>00- .
.00. /
Success00/ 6
(006 7
response007 ?
)00? @
;00@ A
}11 
}22 Õ	
xD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Lessons\Commands\CreateLesson\CreateLessonCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Lessons' .
.. /
Commands/ 7
.7 8
CreateLesson8 D
;D E
public 
record 
CreateLessonCommand !
(! "
string 

Title 
, 
string		 

Content		 
,		 
int

 

OrderIndex

 
,

 
DifficultyLevel 

Difficulty 
, 
Guid 
	SubjectId	 
) 
: 
IRequest 
< 
Result %
<% &
LessonResponse& 4
>4 5
>5 6
;6 7
public 
record 
LessonResponse 
( 
Guid !
Id" $
,$ %
string& ,
Title- 2
,2 3
DifficultyLevel4 C

DifficultyD N
,N O
GuidP T
	SubjectIdU ^
)^ _
;_ `‰
D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Lessons\Commands\CreateLesson\CreateLessonCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Lessons' .
.. /
Commands/ 7
.7 8
CreateLesson8 D
;D E
public

 
class

 &
CreateLessonCommandHandler

 '
:

( )
IRequestHandler

* 9
<

9 :
CreateLessonCommand

: M
,

M N
Result

O U
<

U V
LessonResponse

V d
>

d e
>

e f
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
&
CreateLessonCommandHandler %
(% &!
IApplicationDbContext& ;
context< C
)C D
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
LessonResponse +
>+ ,
>, -
Handle. 4
(4 5
CreateLessonCommand5 H
requestI P
,P Q
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
subjectExists 
= 
await !
_context" *
.* +
Subjects+ 3
. 
AnyAsync 
( 
s 
=> 
s 
. 
Id 
==  "
request# *
.* +
	SubjectId+ 4
,4 5
cancellationToken6 G
)G H
;H I
if 

( 
! 
subjectExists 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
Subject/ 6
)6 7
,7 8
request9 @
.@ A
	SubjectIdA J
)J K
;K L
var 
lesson 
= 
Lesson 
. 
Create "
(" #
request 
. 
Title 
, 
request 
. 
Content 
, 
request 
. 

OrderIndex 
, 
request   
.   

Difficulty   
,   
request!! 
.!! 
	SubjectId!! 
)!! 
;!! 
_context## 
.## 
Lessons## 
.## 
Add## 
(## 
lesson## #
)### $
;##$ %
await$$ 
_context$$ 
.$$ 
SaveChangesAsync$$ '
($$' (
cancellationToken$$( 9
)$$9 :
;$$: ;
return&& 
Result&& 
<&& 
LessonResponse&& $
>&&$ %
.&&% &
Success&&& -
(&&- .
new'' 
LessonResponse'' 
('' 
lesson'' %
.''% &
Id''& (
,''( )
lesson''* 0
.''0 1
Title''1 6
,''6 7
lesson''8 >
.''> ?

Difficulty''? I
,''I J
lesson''K Q
.''Q R
	SubjectId''R [
)''[ \
,''\ ]
$num''^ a
)''a b
;''b c
}(( 
})) “
ÅD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Lessons\Commands\CreateLesson\CreateLessonCommandValidator.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Lessons' .
.. /
Commands/ 7
.7 8
CreateLesson8 D
;D E
public 
class (
CreateLessonCommandValidator )
:* +
AbstractValidator, =
<= >
CreateLessonCommand> Q
>Q R
{ 
public 
(
CreateLessonCommandValidator '
(' (
)( )
{ 
RuleFor		 
(		 
x		 
=>		 
x		 
.		 
Title		 
)		 
.

 
NotEmpty

 
(

 
)

 
.

 
WithMessage

 #
(

# $
$str

$ 8
)

8 9
. 
MaximumLength 
( 
$num 
) 
.  
WithMessage  +
(+ ,
$str, S
)S T
;T U
RuleFor 
( 
x 
=> 
x 
. 
Content 
) 
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ :
): ;
;; <
RuleFor 
( 
x 
=> 
x 
. 

OrderIndex !
)! "
. 
GreaterThan 
( 
$num 
) 
. 
WithMessage '
(' (
$str( M
)M N
;N O
RuleFor 
( 
x 
=> 
x 
. 
	SubjectId  
)  !
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ :
): ;
;; <
} 
} Ç	
wD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Lessons\Queries\GetLessonById\GetLessonByIdQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Lessons' .
.. /
Queries/ 6
.6 7
GetLessonById7 D
;D E
public 
record 
GetLessonByIdQuery  
(  !
Guid! %
LessonId& .
). /
:0 1
IRequest2 :
<: ;
Result; A
<A B
	LessonDtoB K
>K L
>L M
;M N
public		 
record		 
	LessonDto		 
(		 
Guid

 
Id

	 
,

 
string 

Title 
, 
string 

Content 
, 
int 

OrderIndex 
, 
DifficultyLevel 

Difficulty 
, 
Guid 
	SubjectId	 
, 
DateTime 
	CreatedAt 
) 
; £
~D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Lessons\Queries\GetLessonById\GetLessonByIdQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Lessons' .
.. /
Queries/ 6
.6 7
GetLessonById7 D
;D E
public		 
class		 %
GetLessonByIdQueryHandler		 &
:		' (
IRequestHandler		) 8
<		8 9
GetLessonByIdQuery		9 K
,		K L
Result		M S
<		S T
	LessonDto		T ]
>		] ^
>		^ _
{

 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
%
GetLessonByIdQueryHandler $
($ %!
IApplicationDbContext% :
context; B
)B C
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
	LessonDto &
>& '
>' (
Handle) /
(/ 0
GetLessonByIdQuery0 B
requestC J
,J K
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
lesson 
= 
await 
_context #
.# $
Lessons$ +
. 
AsNoTracking 
( 
) 
. 
FirstOrDefaultAsync  
(  !
l! "
=># %
l& '
.' (
Id( *
==+ -
request. 5
.5 6
LessonId6 >
,> ?
cancellationToken@ Q
)Q R
;R S
if 

( 
lesson 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
lesson/ 5
)5 6
,6 7
request8 ?
.? @
LessonId@ H
)H I
;I J
return 
Result 
< 
	LessonDto 
>  
.  !
Success! (
(( )
new) ,
	LessonDto- 6
(6 7
lesson 
. 
Id 
, 
lesson 
. 
Title 
, 
lesson 
. 
Content 
, 
lesson   
.   

OrderIndex   
,   
lesson!! 
.!! 

Difficulty!! 
,!! 
lesson"" 
."" 
	SubjectId"" 
,"" 
lesson## 
.## 
	CreatedAt## 
)## 
)## 
;## 
}$$ 
}%% Ã	
ÉD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Lessons\Queries\GetLessonsBySubject\GetLessonsBySubjectQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Lessons' .
.. /
Queries/ 6
.6 7
GetLessonsBySubject7 J
;J K
public 
record $
GetLessonsBySubjectQuery &
(& '
Guid' +
	SubjectId, 5
)5 6
:7 8
IRequest9 A
<A B
ResultB H
<H I
ListI M
<M N
	LessonDtoN W
>W X
>X Y
>Y Z
;Z [
public		 
record		 
	LessonDto		 
(		 
Guid

 
Id

	 
,

 
string 

Title 
, 
string 

Content 
, 
int 

OrderIndex 
, 
DifficultyLevel 

Difficulty 
, 
Guid 
	SubjectId	 
, 
DateTime 
	CreatedAt 
) 
; ñ
äD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Lessons\Queries\GetLessonsBySubject\GetLessonsBySubjectQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Lessons' .
.. /
Queries/ 6
.6 7
GetLessonsBySubject7 J
;J K
public 
class +
GetLessonsBySubjectQueryHandler ,
:		 
IRequestHandler		 
<		 $
GetLessonsBySubjectQuery		 .
,		. /
Result		0 6
<		6 7
List		7 ;
<		; <
	LessonDto		< E
>		E F
>		F G
>		G H
{

 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
+
GetLessonsBySubjectQueryHandler *
(* +!
IApplicationDbContext+ @
contextA H
)H I
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
List !
<! "
	LessonDto" +
>+ ,
>, -
>- .
Handle/ 5
(5 6$
GetLessonsBySubjectQuery  
request! (
,( )
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
lessons 
= 
await 
_context $
.$ %
Lessons% ,
. 
AsNoTracking 
( 
) 
. 
Where 
( 
l 
=> 
l 
. 
	SubjectId #
==$ &
request' .
.. /
	SubjectId/ 8
)8 9
. 
OrderBy 
( 
l 
=> 
l 
. 

OrderIndex &
)& '
. 
Select 
( 
l 
=> 
new 
	LessonDto &
(& '
l 
. 
Id 
, 
l 
. 
Title 
, 
l 
. 
Content 
, 
l 
. 

OrderIndex 
, 
l 
. 

Difficulty 
, 
l   
.   
	SubjectId   
,   
l!! 
.!! 
	CreatedAt!! 
)!! 
)!! 
."" 
ToListAsync"" 
("" 
cancellationToken"" *
)""* +
;""+ ,
return$$ 
Result$$ 
<$$ 
List$$ 
<$$ 
	LessonDto$$ $
>$$$ %
>$$% &
.$$& '
Success$$' .
($$. /
lessons$$/ 6
)$$6 7
;$$7 8
}%% 
}&& Ù
}D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Progress\Commands\CompleteLesson\CompleteLessonCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Progress' /
./ 0
Commands0 8
.8 9
CompleteLesson9 G
;G H
public 
record !
CompleteLessonCommand #
(# $
Guid 
UserId	 
, 
Guid 
LessonId	 
, 
int		 
ScorePercentage		 
)		 
:		 
IRequest		 #
<		# $
Result		$ *
<		* +
ProgressResponse		+ ;
>		; <
>		< =
;		= >
public 
record 
ProgressResponse 
( 
Guid #
Id$ &
,& '
bool( ,
IsCompleted- 8
,8 9
int: =
ScorePercentage> M
,M N
DateTimeO W
?W X
CompletedAtY d
)d e
;e fã
ÑD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Progress\Commands\CompleteLesson\CompleteLessonCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Progress' /
./ 0
Commands0 8
.8 9
CompleteLesson9 G
;G H
public		 
class		 (
CompleteLessonCommandHandler		 )
:		* +
IRequestHandler		, ;
<		; <!
CompleteLessonCommand		< Q
,		Q R
Result		S Y
<		Y Z
ProgressResponse		Z j
>		j k
>		k l
{

 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
(
CompleteLessonCommandHandler '
(' (!
IApplicationDbContext( =
context> E
)E F
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
ProgressResponse -
>- .
>. /
Handle0 6
(6 7!
CompleteLessonCommand7 L
requestM T
,T U
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
progress 
= 
await 
_context %
.% &
StudentProgresses& 7
. 
FirstOrDefaultAsync  
(  !
p! "
=># %
p& '
.' (
UserId( .
==/ 1
request2 9
.9 :
UserId: @
&& 
p 
. 
LessonId 
==  
request! (
.( )
LessonId) 1
,1 2
cancellationToken3 D
)D E
;E F
if 

( 
progress 
is 
null 
) 
{ 	
progress 
= 
StudentProgress &
.& '
Create' -
(- .
request. 5
.5 6
UserId6 <
,< =
request> E
.E F
LessonIdF N
)N O
;O P
_context 
. 
StudentProgresses &
.& '
Add' *
(* +
progress+ 3
)3 4
;4 5
} 	
progress 
. 
Complete 
( 
request !
.! "
ScorePercentage" 1
)1 2
;2 3
await   
_context   
.   
SaveChangesAsync   '
(  ' (
cancellationToken  ( 9
)  9 :
;  : ;
return"" 
Result"" 
<"" 
ProgressResponse"" &
>""& '
.""' (
Success""( /
(""/ 0
new## 
ProgressResponse##  
(##  !
progress$$ 
.$$ 
Id$$ 
,$$ 
progress%% 
.%% 
IsCompleted%% $
,%%$ %
progress&& 
.&& 
ScorePercentage&& (
,&&( )
progress'' 
.'' 
CompletedAt'' $
)''$ %
)''% &
;''& '
}(( 
})) ™
ÜD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Progress\Commands\CompleteLesson\CompleteLessonCommandValidator.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Progress' /
./ 0
Commands0 8
.8 9
CompleteLesson9 G
;G H
public 
class *
CompleteLessonCommandValidator +
:, -
AbstractValidator. ?
<? @!
CompleteLessonCommand@ U
>U V
{ 
public 
*
CompleteLessonCommandValidator )
() *
)* +
{ 
RuleFor		 
(		 
x		 
=>		 
x		 
.		 
UserId		 
)		 
.

 
NotEmpty

 
(

 
)

 
.

 
WithMessage

 #
(

# $
$str

$ 7
)

7 8
;

8 9
RuleFor 
( 
x 
=> 
x 
. 
LessonId 
)  
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ 9
)9 :
;: ;
RuleFor 
( 
x 
=> 
x 
. 
ScorePercentage &
)& '
. 
InclusiveBetween 
( 
$num 
,  
$num! $
)$ %
.% &
WithMessage& 1
(1 2
$str2 T
)T U
;U V
} 
} ¬
rD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Progress\Events\LessonCompletedEventHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Progress' /
./ 0
Events0 6
;6 7
public 
class '
LessonCompletedEventHandler (
:) * 
INotificationHandler+ ?
<? @ 
LessonCompletedEvent@ T
>T U
{ 
private		 
readonly		 
ILogger		 
<		 '
LessonCompletedEventHandler		 8
>		8 9
_logger		: A
;		A B
public 
'
LessonCompletedEventHandler &
(& '
ILogger' .
<. /'
LessonCompletedEventHandler/ J
>J K
loggerL R
)R S
{ 
_logger 
= 
logger 
; 
} 
public 

Task 
Handle 
(  
LessonCompletedEvent +
notification, 8
,8 9
CancellationToken: K
cancellationTokenL ]
)] ^
{ 
_logger 
. 
LogInformation 
( 
$str N
,N O
notification 
. 
LessonId !
,! "
notification 
. 
UserId 
,  
notification 
. 
ScorePercentage (
)( )
;) *
return 
Task 
. 
CompletedTask !
;! "
} 
} À
nD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Progress\Events\UserCreatedEventHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Events- 3
;3 4
public 
class #
UserCreatedEventHandler $
:% & 
INotificationHandler' ;
<; <
UserCreatedEvent< L
>L M
{ 
private		 
readonly		 
ILogger		 
<		 #
UserCreatedEventHandler		 4
>		4 5
_logger		6 =
;		= >
public 
#
UserCreatedEventHandler "
(" #
ILogger# *
<* +#
UserCreatedEventHandler+ B
>B C
loggerD J
)J K
{ 
_logger 
= 
logger 
; 
} 
public 

Task 
Handle 
( 
UserCreatedEvent '
notification( 4
,4 5
CancellationToken6 G
cancellationTokenH Y
)Y Z
{ 
_logger 
. 
LogInformation 
( 
$str 6
,6 7
notification 
. 
UserId 
,  
notification 
. 
Email 
) 
;  
return 
Task 
. 
CompletedTask !
;! "
} 
} º	
ÇD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Progress\Queries\GetStudentProgress\GetStudentProgressQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Progress' /
./ 0
Queries0 7
.7 8
GetStudentProgress8 J
;J K
public 
record #
GetStudentProgressQuery %
(% &
Guid& *
UserId+ 1
)1 2
:3 4
IRequest5 =
<= >
Result> D
<D E
ListE I
<I J
StudentProgressDtoJ \
>\ ]
>] ^
>^ _
;_ `
public 
record 
StudentProgressDto  
(  !
Guid		 
LessonId			 
,		 
string

 

LessonTitle

 
,

 
bool 
IsCompleted	 
, 
int 
ScorePercentage 
, 
int 
AttemptsCount 
, 
DateTime 
? 
CompletedAt 
) 
; Æ
âD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Progress\Queries\GetStudentProgress\GetStudentProgressQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Progress' /
./ 0
Queries0 7
.7 8
GetStudentProgress8 J
;J K
public 
class *
GetStudentProgressQueryHandler +
:		 
IRequestHandler		 
<		 #
GetStudentProgressQuery		 -
,		- .
Result		/ 5
<		5 6
List		6 :
<		: ;
StudentProgressDto		; M
>		M N
>		N O
>		O P
{

 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
*
GetStudentProgressQueryHandler )
() *!
IApplicationDbContext* ?
context@ G
)G H
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
List !
<! "
StudentProgressDto" 4
>4 5
>5 6
>6 7
Handle8 >
(> ?#
GetStudentProgressQuery? V
requestW ^
,^ _
CancellationToken 
cancellationToken +
)+ ,
{ 
var 

progresses 
= 
await 
_context '
.' (
StudentProgresses( 9
. 
AsNoTracking 
( 
) 
. 
Include 
( 
p 
=> 
p 
. 
Lesson "
)" #
. 
Where 
( 
p 
=> 
p 
. 
UserId  
==! #
request$ +
.+ ,
UserId, 2
)2 3
. 
Select 
( 
p 
=> 
new 
StudentProgressDto /
(/ 0
p 
. 
LessonId 
, 
p 
. 
Lesson 
. 
Title 
, 
p 
. 
IsCompleted 
, 
p 
. 
ScorePercentage !
,! "
p 
. 
AttemptsCount 
,  
p 
. 
CompletedAt 
) 
) 
.   
ToListAsync   
(   
cancellationToken   *
)  * +
;  + ,
return"" 
Result"" 
<"" 
List"" 
<"" 
StudentProgressDto"" -
>""- .
>"". /
.""/ 0
Success""0 7
(""7 8

progresses""8 B
)""B C
;""C D
}## 
}$$ á
ÖD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Questions\Commands\CreateQuestion\CreateQuestionCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
	Questions' 0
.0 1
Commands1 9
.9 :
CreateQuestion: H
;H I
public

 
class

 (
CreateQuestionCommandHandler

 )
: 
IRequestHandler 
< !
CreateQuestionCommand +
,+ ,
Result- 3
<3 4
QuestionResponse4 D
>D E
>E F
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
(
CreateQuestionCommandHandler '
(' (!
IApplicationDbContext( =
context> E
)E F
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
QuestionResponse -
>- .
>. /
Handle0 6
(6 7!
CreateQuestionCommand7 L
requestM T
,T U
CancellationToken 
cancellationToken +
)+ ,
{ 
var 

quizExists 
= 
await 
_context '
.' (
Quizzes( /
. 
AnyAsync 
( 
q 
=> 
q 
. 
Id 
==  "
request# *
.* +
QuizId+ 1
,1 2
cancellationToken3 D
)D E
;E F
if 

( 
! 

quizExists 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
Quiz/ 3
)3 4
,4 5
request6 =
.= >
QuizId> D
)D E
;E F
var 
question 
= 
Question 
.  
Create  &
(& '
request 
. 
Text 
, 
request 
. 
CorrectAnswer !
,! "
request   
.   
Options   
,   
request!! 
.!! 
Points!! 
,!! 
request"" 
."" 
QuizId"" 
,"" 
request## 
.## 
Explanation## 
)##  
;##  !
_context%% 
.%% 
	Questions%% 
.%% 
Add%% 
(%% 
question%% '
)%%' (
;%%( )
await&& 
_context&& 
.&& 
SaveChangesAsync&& '
(&&' (
cancellationToken&&( 9
)&&9 :
;&&: ;
return(( 
Result(( 
<(( 
QuestionResponse(( &
>((& '
.((' (
Success((( /
(((/ 0
new)) 
QuestionResponse))  
())  !
question** 
.** 
Id** 
,** 
question++ 
.++ 
Text++ 
,++ 
question,, 
.,, 
Options,,  
,,,  !
question-- 
.-- 
Points-- 
,--  
question.. 
... 
QuizId.. 
)..  
,..  !
$num.." %
)..% &
;..& '
}// 
}00 π
xD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Quizzes\Commands\CreateQuiz\CreateQuestionCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
	Questions' 0
.0 1
Commands1 9
.9 :
CreateQuestion: H
;H I
public 
record !
CreateQuestionCommand #
(# $
string 

Text 
, 
string 

CorrectAnswer 
, 
List		 
<		 	
string			 
>		 
Options		 
,		 
int

 
Points

 
,

 
Guid 
QuizId	 
, 
string 

?
 
Explanation 
= 
null 
) 
:  !
IRequest" *
<* +
Result+ 1
<1 2
QuestionResponse2 B
>B C
>C D
;D E
public 
record 
QuestionResponse 
( 
Guid 
Id	 
, 
string 

Text 
, 
List 
< 	
string	 
> 
Options 
, 
int 
Points 
, 
Guid 
QuizId	 
) 
; ¶
ÅD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Quizzes\Commands\CreateQuiz\CreateQuestionCommandValidator.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
	Questions' 0
.0 1
Commands1 9
.9 :
CreateQuestion: H
;H I
public 
class *
CreateQuestionCommandValidator +
:, -
AbstractValidator. ?
<? @!
CreateQuestionCommand@ U
>U V
{ 
public 
*
CreateQuestionCommandValidator )
() *
)* +
{ 
RuleFor		 
(		 
x		 
=>		 
x		 
.		 
Text		 
)		 
.

 
NotEmpty

 
(

 
)

 
.

 
WithMessage

 #
(

# $
$str

$ @
)

@ A
. 
MaximumLength 
( 
$num 
)  
.  !
WithMessage! ,
(, -
$str- ]
)] ^
;^ _
RuleFor 
( 
x 
=> 
x 
. 
CorrectAnswer $
)$ %
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ A
)A B
;B C
RuleFor 
( 
x 
=> 
x 
. 
Options 
) 
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ ;
); <
. 
Must 
( 
o 
=> 
o 
. 
Count 
>= !
$num" #
)# $
.$ %
WithMessage% 0
(0 1
$str1 S
)S T
. 
Must 
( 
o 
=> 
o 
. 
Count 
<= !
$num" #
)# $
.$ %
WithMessage% 0
(0 1
$str1 M
)M N
;N O
RuleFor 
( 
x 
=> 
x 
. 
Points 
) 
. 
GreaterThan 
( 
$num 
) 
. 
WithMessage '
(' (
$str( H
)H I
. 
LessThanOrEqualTo 
( 
$num "
)" #
.# $
WithMessage$ /
(/ 0
$str0 M
)M N
;N O
RuleFor 
( 
x 
=> 
x 
. 
QuizId 
) 
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ 7
)7 8
;8 9
RuleFor 
( 
x 
=> 
x 
) 
. 
Must 
( 
x 
=> 
x 
. 
Options  
.  !
Contains! )
() *
x* +
.+ ,
CorrectAnswer, 9
)9 :
): ;
. 
WithMessage 
( 
$str E
)E F
;F G
} 
}   À	
tD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Quizzes\Commands\CreateQuiz\CreateQuizCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Quizzes' .
.. /
Commands/ 7
.7 8

CreateQuiz8 B
;B C
public 
record 
CreateQuizCommand 
(  
string 

Title 
, 
DifficultyLevel		 

Difficulty		 
,		 
Guid

 
LessonId

	 
,

 
int 
TimeLimitMinutes 
) 
: 
IRequest $
<$ %
Result% +
<+ ,
QuizResponse, 8
>8 9
>9 :
;: ;
public 
record 
QuizResponse 
( 
Guid 
Id	 
, 
string 

Title 
, 
DifficultyLevel 

Difficulty 
, 
Guid 
LessonId	 
, 
int 
TimeLimitMinutes 
) 
; ƒ
{D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Quizzes\Commands\CreateQuiz\CreateQuizCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Quizzes' .
.. /
Commands/ 7
.7 8

CreateQuiz8 B
;B C
public

 
class

 $
CreateQuizCommandHandler

 %
:

& '
IRequestHandler

( 7
<

7 8
CreateQuizCommand

8 I
,

I J
Result

K Q
<

Q R
QuizResponse

R ^
>

^ _
>

_ `
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
$
CreateQuizCommandHandler #
(# $!
IApplicationDbContext$ 9
context: A
)A B
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
QuizResponse )
>) *
>* +
Handle, 2
(2 3
CreateQuizCommand3 D
requestE L
,L M
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
lessonExists 
= 
await  
_context! )
.) *
Lessons* 1
. 
AnyAsync 
( 
l 
=> 
l 
. 
Id 
==  "
request# *
.* +
LessonId+ 3
,3 4
cancellationToken5 F
)F G
;G H
if 

( 
! 
lessonExists 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
Lesson/ 5
)5 6
,6 7
request8 ?
.? @
LessonId@ H
)H I
;I J
var 
quiz 
= 
Quiz 
. 
Create 
( 
request 
. 
Title 
, 
request 
. 

Difficulty 
, 
request 
. 
LessonId 
, 
request   
.   
TimeLimitMinutes   $
)  $ %
;  % &
_context"" 
."" 
Quizzes"" 
."" 
Add"" 
("" 
quiz"" !
)""! "
;""" #
await## 
_context## 
.## 
SaveChangesAsync## '
(##' (
cancellationToken##( 9
)##9 :
;##: ;
return%% 
Result%% 
<%% 
QuizResponse%% "
>%%" #
.%%# $
Success%%$ +
(%%+ ,
new&& 
QuizResponse&& 
(&& 
quiz'' 
.'' 
Id'' 
,'' 
quiz(( 
.(( 
Title(( 
,(( 
quiz)) 
.)) 

Difficulty)) 
,))  
quiz** 
.** 
LessonId** 
,** 
quiz++ 
.++ 
TimeLimitMinutes++ %
)++% &
,++& '
$num++( +
)+++ ,
;++, -
},, 
}-- —
}D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Quizzes\Commands\CreateQuiz\CreateQuizCommandValidator.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Quizzes' .
.. /
Commands/ 7
.7 8

CreateQuiz8 B
;B C
public 
class &
CreateQuizCommandValidator '
:( )
AbstractValidator* ;
<; <
CreateQuizCommand< M
>M N
{ 
public 
&
CreateQuizCommandValidator %
(% &
)& '
{ 
RuleFor		 
(		 
x		 
=>		 
x		 
.		 
Title		 
)		 
.

 
NotEmpty

 
(

 
)

 
.

 
WithMessage

 #
(

# $
$str

$ 8
)

8 9
. 
MaximumLength 
( 
$num 
) 
.  
WithMessage  +
(+ ,
$str, S
)S T
;T U
RuleFor 
( 
x 
=> 
x 
. 
LessonId 
)  
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ 9
)9 :
;: ;
RuleFor 
( 
x 
=> 
x 
. 
TimeLimitMinutes '
)' (
. 
GreaterThan 
( 
$num 
) 
. 
WithMessage '
(' (
$str( L
)L M
. 
LessThanOrEqualTo 
( 
$num "
)" #
.# $
WithMessage$ /
(/ 0
$str0 Y
)Y Z
;Z [
} 
} ◊
tD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Quizzes\Commands\SubmitQuiz\SubmitQuizCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Quizzes' .
.. /
Commands/ 7
.7 8

SubmitQuiz8 B
;B C
public 
record 
SubmitQuizCommand 
(  
Guid 
QuizId	 
, 
Guid 
UserId	 
, 
List		 
<		 	
QuizAnswerDto			 
>		 
Answers		 
)		  
:		! "
IRequest		# +
<		+ ,
Result		, 2
<		2 3
QuizResultResponse		3 E
>		E F
>		F G
;		G H
public 
record 
QuizAnswerDto 
( 
Guid  

QuestionId! +
,+ ,
string- 3
Answer4 :
): ;
;; <
public 
record 
QuizResultResponse  
(  !
Guid 
QuizId	 
, 
Guid 
UserId	 
, 
int 
TotalQuestions 
, 
int 
CorrectAnswers 
, 
int 
ScorePercentage 
, 
List 
< 	
QuestionResultDto	 
> 
Results #
)# $
;$ %
public 
record 
QuestionResultDto 
(  
Guid 

QuestionId	 
, 
string 

QuestionText 
, 
string 


UserAnswer 
, 
string 

CorrectAnswer 
, 
bool 
	IsCorrect	 
, 
string 

?
 
Explanation 
) 
; ç*
{D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Quizzes\Commands\SubmitQuiz\SubmitQuizCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Quizzes' .
.. /
Commands/ 7
.7 8

SubmitQuiz8 B
;B C
public		 
class		 $
SubmitQuizCommandHandler		 %
:		& '
IRequestHandler		( 7
<		7 8
SubmitQuizCommand		8 I
,		I J
Result		K Q
<		Q R
QuizResultResponse		R d
>		d e
>		e f
{

 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
$
SubmitQuizCommandHandler #
(# $!
IApplicationDbContext$ 9
context: A
)A B
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
QuizResultResponse /
>/ 0
>0 1
Handle2 8
(8 9
SubmitQuizCommand9 J
requestK R
,R S
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
quiz 
= 
await 
_context !
.! "
Quizzes" )
. 
Include 
( 
q 
=> 
q 
. 
	Questions %
)% &
. 
FirstOrDefaultAsync  
(  !
q! "
=># %
q& '
.' (
Id( *
==+ -
request. 5
.5 6
QuizId6 <
,< =
cancellationToken> O
)O P
;P Q
if 

( 
quiz 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
quiz/ 3
)3 4
,4 5
request6 =
.= >
QuizId> D
)D E
;E F
var 
results 
= 
new 
List 
< 
QuestionResultDto 0
>0 1
(1 2
)2 3
;3 4
int 
correctAnswers 
= 
$num 
; 
foreach 
( 
var 
answer 
in 
request &
.& '
Answers' .
). /
{   	
var!! 
question!! 
=!! 
quiz!! 
.!!  
	Questions!!  )
."" 
FirstOrDefault"" 
(""  
q""  !
=>""" $
q""% &
.""& '
Id""' )
==""* ,
answer""- 3
.""3 4

QuestionId""4 >
)""> ?
;""? @
if$$ 
($$ 
question$$ 
is$$ 
null$$  
)$$  !
continue$$" *
;$$* +
var&& 
	isCorrect&& 
=&& 
question&& $
.&&$ %
	IsCorrect&&% .
(&&. /
answer&&/ 5
.&&5 6
Answer&&6 <
)&&< =
;&&= >
if'' 
('' 
	isCorrect'' 
)'' 
correctAnswers'' )
++'') +
;''+ ,
results)) 
.)) 
Add)) 
()) 
new)) 
QuestionResultDto)) -
())- .
question** 
.** 
Id** 
,** 
question++ 
.++ 
Text++ 
,++ 
answer,, 
.,, 
Answer,, 
,,, 
question-- 
.-- 
CorrectAnswer-- &
,--& '
	isCorrect.. 
,.. 
question// 
.// 
Explanation// $
)//$ %
)//% &
;//& '
}00 	
var22 
totalQuestions22 
=22 
quiz22 !
.22! "
	Questions22" +
.22+ ,
Count22, 1
;221 2
var33 
scorePercentage33 
=33 
totalQuestions33 ,
>33- .
$num33/ 0
?44 
(44 
int44 
)44 
Math44 
.44 
Round44 
(44 
(44 
double44 %
)44% &
correctAnswers44& 4
/445 6
totalQuestions447 E
*44F G
$num44H K
)44K L
:55 
$num55 
;55 
return77 
Result77 
<77 
QuizResultResponse77 (
>77( )
.77) *
Success77* 1
(771 2
new772 5
QuizResultResponse776 H
(77H I
request88 
.88 
QuizId88 
,88 
request99 
.99 
UserId99 
,99 
totalQuestions:: 
,:: 
correctAnswers;; 
,;; 
scorePercentage<< 
,<< 
results== 
)== 
)== 
;== 
}>> 
}?? “
}D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Quizzes\Commands\SubmitQuiz\SubmitQuizCommandValidator.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Quizzes' .
.. /
Commands/ 7
.7 8

SubmitQuiz8 B
;B C
public 
class &
SubmitQuizCommandValidator '
:( )
AbstractValidator* ;
<; <
SubmitQuizCommand< M
>M N
{ 
public 
&
SubmitQuizCommandValidator %
(% &
)& '
{ 
RuleFor		 
(		 
x		 
=>		 
x		 
.		 
QuizId		 
)		 
.

 
NotEmpty

 
(

 
)

 
.

 
WithMessage

 #
(

# $
$str

$ 7
)

7 8
;

8 9
RuleFor 
( 
x 
=> 
x 
. 
UserId 
) 
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ 7
)7 8
;8 9
RuleFor 
( 
x 
=> 
x 
. 
Answers 
) 
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ ;
); <
. 
Must 
( 
a 
=> 
a 
. 
Count 
>  
$num! "
)" #
.# $
WithMessage$ /
(/ 0
$str0 R
)R S
;S T
} 
} ﬂ
sD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Quizzes\Queries\GetQuizById\GetQuizByIdQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Quizzes' .
.. /
Queries/ 6
.6 7
GetQuizById7 B
;B C
public 
record 
GetQuizByIdQuery 
( 
Guid #
QuizId$ *
)* +
:, -
IRequest. 6
<6 7
Result7 =
<= >
QuizDetailDto> K
>K L
>L M
;M N
public		 
record		 
QuizDetailDto		 
(		 
Guid

 
Id

	 
,

 
string 

Title 
, 
DifficultyLevel 

Difficulty 
, 
Guid 
LessonId	 
, 
int 
TimeLimitMinutes 
, 
List 
< 	
QuestionDto	 
> 
	Questions 
)  
;  !
public 
record 
QuestionDto 
( 
Guid 
Id	 
, 
string 

Text 
, 
List 
< 	
string	 
> 
Options 
, 
int 
Points 
) 
; ü
zD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Quizzes\Queries\GetQuizById\GetQuizByIdQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Quizzes' .
.. /
Queries/ 6
.6 7
GetQuizById7 B
;B C
public		 
class		 #
GetQuizByIdQueryHandler		 $
:		% &
IRequestHandler		' 6
<		6 7
GetQuizByIdQuery		7 G
,		G H
Result		I O
<		O P
QuizDetailDto		P ]
>		] ^
>		^ _
{

 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
#
GetQuizByIdQueryHandler "
(" #!
IApplicationDbContext# 8
context9 @
)@ A
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
QuizDetailDto *
>* +
>+ ,
Handle- 3
(3 4
GetQuizByIdQuery4 D
requestE L
,L M
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
quiz 
= 
await 
_context !
.! "
Quizzes" )
. 
AsNoTracking 
( 
) 
. 
Include 
( 
q 
=> 
q 
. 
	Questions %
)% &
. 
FirstOrDefaultAsync  
(  !
q! "
=># %
q& '
.' (
Id( *
==+ -
request. 5
.5 6
QuizId6 <
,< =
cancellationToken> O
)O P
;P Q
if 

( 
quiz 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
quiz/ 3
)3 4
,4 5
request6 =
.= >
QuizId> D
)D E
;E F
var 
questionDtos 
= 
quiz 
.  
	Questions  )
.) *
Select* 0
(0 1
q1 2
=>3 5
new6 9
QuestionDto: E
(E F
q 
. 
Id 
, 
q 
. 
Text 
, 
q   
.   
Options   
,   
q!! 
.!! 
Points!! 
)!! 
)!! 
.!! 
ToList!! 
(!! 
)!! 
;!!  
return## 
Result## 
<## 
QuizDetailDto## #
>### $
.##$ %
Success##% ,
(##, -
new##- 0
QuizDetailDto##1 >
(##> ?
quiz$$ 
.$$ 
Id$$ 
,$$ 
quiz%% 
.%% 
Title%% 
,%% 
quiz&& 
.&& 

Difficulty&& 
,&& 
quiz'' 
.'' 
LessonId'' 
,'' 
quiz(( 
.(( 
TimeLimitMinutes(( !
,((! "
questionDtos)) 
))) 
))) 
;)) 
}** 
}++ Á
ÅD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Quizzes\Queries\GetQuizzesByLesson\GetQuizzesByLessonQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Quizzes' .
.. /
Queries/ 6
.6 7
GetQuizzesByLesson7 I
;I J
public 
record #
GetQuizzesByLessonQuery %
(% &
Guid& *
LessonId+ 3
)3 4
:5 6
IRequest7 ?
<? @
Result@ F
<F G
ListG K
<K L
QuizSummaryDtoL Z
>Z [
>[ \
>\ ]
;] ^
public		 
record		 
QuizSummaryDto		 
(		 
Guid

 
Id

	 
,

 
string 

Title 
, 
DifficultyLevel 

Difficulty 
, 
int 
TimeLimitMinutes 
, 
int 
QuestionsCount 
) 
; ¬
àD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Quizzes\Queries\GetQuizzesByLesson\GetQuizzesByLessonQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Quizzes' .
.. /
Queries/ 6
.6 7
GetQuizzesByLesson7 I
;I J
public 
class *
GetQuizzesByLessonQueryHandler +
:		 
IRequestHandler		 
<		 #
GetQuizzesByLessonQuery		 -
,		- .
Result		/ 5
<		5 6
List		6 :
<		: ;
QuizSummaryDto		; I
>		I J
>		J K
>		K L
{

 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
*
GetQuizzesByLessonQueryHandler )
() *!
IApplicationDbContext* ?
context@ G
)G H
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
List !
<! "
QuizSummaryDto" 0
>0 1
>1 2
>2 3
Handle4 :
(: ;#
GetQuizzesByLessonQuery; R
requestS Z
,Z [
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
quizzes 
= 
await 
_context $
.$ %
Quizzes% ,
. 
AsNoTracking 
( 
) 
. 
Include 
( 
q 
=> 
q 
. 
	Questions %
)% &
. 
Where 
( 
q 
=> 
q 
. 
LessonId "
==# %
request& -
.- .
LessonId. 6
)6 7
. 
Select 
( 
q 
=> 
new 
QuizSummaryDto +
(+ ,
q 
. 
Id 
, 
q 
. 
Title 
, 
q 
. 

Difficulty 
, 
q 
. 
TimeLimitMinutes "
," #
q 
. 
	Questions 
. 
Count !
)! "
)" #
. 
ToListAsync 
( 
cancellationToken *
)* +
;+ ,
return!! 
Result!! 
<!! 
List!! 
<!! 
QuizSummaryDto!! )
>!!) *
>!!* +
.!!+ ,
Success!!, 3
(!!3 4
quizzes!!4 ;
)!!; <
;!!< =
}"" 
}## õ
{D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subjects\Commands\CreateSubject\CreateSubjectCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subjects' /
./ 0
Commands0 8
.8 9
CreateSubject9 F
;F G
public 
record  
CreateSubjectCommand "
(" #
string 

Name 
, 
string		 

Description		 
,		 
SubjectType

 
Type

 
)

 
:

 
IRequest

  
<

  !
Result

! '
<

' (
SubjectResponse

( 7
>

7 8
>

8 9
;

9 :
public 
record 
SubjectResponse 
( 
Guid "
Id# %
,% &
string' -
Name. 2
,2 3
SubjectType4 ?
Type@ D
)D E
;E Fó
ÇD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subjects\Commands\CreateSubject\CreateSubjectCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subjects' /
./ 0
Commands0 8
.8 9
CreateSubject9 F
;F G
public 
class '
CreateSubjectCommandHandler (
:) *
IRequestHandler+ :
<: ; 
CreateSubjectCommand; O
,O P
ResultQ W
<W X
SubjectResponseX g
>g h
>h i
{		 
private

 
readonly

 !
IApplicationDbContext

 *
_context

+ 3
;

3 4
public 
'
CreateSubjectCommandHandler &
(& '!
IApplicationDbContext' <
context= D
)D E
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
SubjectResponse ,
>, -
>- .
Handle/ 5
(5 6 
CreateSubjectCommand6 J
requestK R
,R S
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
subject 
= 
Subject 
. 
Create $
($ %
request% ,
., -
Name- 1
,1 2
request3 :
.: ;
Description; F
,F G
requestH O
.O P
TypeP T
)T U
;U V
_context 
. 
Subjects 
. 
Add 
( 
subject %
)% &
;& '
await 
_context 
. 
SaveChangesAsync '
(' (
cancellationToken( 9
)9 :
;: ;
return 
Result 
< 
SubjectResponse %
>% &
.& '
Success' .
(. /
new 
SubjectResponse 
(  
subject  '
.' (
Id( *
,* +
subject, 3
.3 4
Name4 8
,8 9
subject: A
.A B
TypeB F
)F G
,G H
$numI L
)L M
;M N
} 
} ó
ÑD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subjects\Commands\CreateSubject\CreateSubjectCommandValidator.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subjects' /
./ 0
Commands0 8
.8 9
CreateSubject9 F
;F G
public 
class )
CreateSubjectCommandValidator *
:+ ,
AbstractValidator- >
<> ? 
CreateSubjectCommand? S
>S T
{ 
public 
)
CreateSubjectCommandValidator (
(( )
)) *
{ 
RuleFor		 
(		 
x		 
=>		 
x		 
.		 
Name		 
)		 
.

 
NotEmpty

 
(

 
)

 
.

 
WithMessage

 #
(

# $
$str

$ 7
)

7 8
. 
MaximumLength 
( 
$num 
) 
.  
WithMessage  +
(+ ,
$str, R
)R S
;S T
RuleFor 
( 
x 
=> 
x 
. 
Description "
)" #
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ >
)> ?
. 
MaximumLength 
( 
$num 
) 
.  
WithMessage  +
(+ ,
$str, Y
)Y Z
;Z [
} 
} ¡
zD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subjects\Queries\GetAllSubjects\GetAllSubjectsQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subjects' /
./ 0
Queries0 7
.7 8
GetAllSubjects8 F
;F G
public 
record 
GetAllSubjectsQuery !
:" #
IRequest$ ,
<, -
Result- 3
<3 4
List4 8
<8 9

SubjectDto9 C
>C D
>D E
>E F
;F G
public		 
record		 

SubjectDto		 
(		 
Guid		 
Id		  
,		  !
string		" (
Name		) -
,		- .
string		/ 5
Description		6 A
,		A B
SubjectType		C N
Type		O S
)		S T
;		T U∂
ÅD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subjects\Queries\GetAllSubjects\GetAllSubjectsQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subjects' /
./ 0
Queries0 7
.7 8
GetAllSubjects8 F
;F G
public 
class &
GetAllSubjectsQueryHandler '
:( )
IRequestHandler* 9
<9 :
GetAllSubjectsQuery: M
,M N
ResultO U
<U V
ListV Z
<Z [

SubjectDto[ e
>e f
>f g
>g h
{		 
private

 
readonly

 !
IApplicationDbContext

 *
_context

+ 3
;

3 4
public 
&
GetAllSubjectsQueryHandler %
(% &!
IApplicationDbContext& ;
context< C
)C D
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
List !
<! "

SubjectDto" ,
>, -
>- .
>. /
Handle0 6
(6 7
GetAllSubjectsQuery7 J
requestK R
,R S
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
subjects 
= 
await 
_context %
.% &
Subjects& .
. 
AsNoTracking 
( 
) 
. 
Select 
( 
s 
=> 
new 

SubjectDto '
(' (
s( )
.) *
Id* ,
,, -
s. /
./ 0
Name0 4
,4 5
s6 7
.7 8
Description8 C
,C D
sE F
.F G
TypeG K
)K L
)L M
. 
ToListAsync 
( 
cancellationToken *
)* +
;+ ,
return 
Result 
< 
List 
< 

SubjectDto %
>% &
>& '
.' (
Success( /
(/ 0
subjects0 8
)8 9
;9 :
} 
} ı
äD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subscriptions\Commands\CancelSubscription\CancelSubscriptionCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subscriptions' 4
.4 5
Commands5 =
.= >
CancelSubscription> P
;P Q
public 
record %
CancelSubscriptionCommand '
(' (
Guid( ,
SubscriptionId- ;
,; <
Guid= A
UserIdB H
)H I
: 
IRequest 
< 
Result 
< 
bool 
> 
> 
; ó&
ëD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subscriptions\Commands\CancelSubscription\CancelSubscriptionCommandHandler.cs
	namespace		 	
AiTutor		
 
.		 
Application		 
.		 
Features		 &
.		& '
Subscriptions		' 4
.		4 5
Commands		5 =
.		= >
CancelSubscription		> P
;		P Q
public 
class ,
 CancelSubscriptionCommandHandler -
:. /
IRequestHandler0 ?
<? @%
CancelSubscriptionCommand@ Y
,Y Z
Result[ a
<a b
boolb f
>f g
>g h
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
private 
readonly 
IStripeService #
_stripe$ +
;+ ,
private 
readonly 
ILogger 
< ,
 CancelSubscriptionCommandHandler =
>= >
_logger? F
;F G
public 
,
 CancelSubscriptionCommandHandler +
(+ ,!
IApplicationDbContext 
context %
,% &
IStripeService 
stripe 
, 
ILogger 
< ,
 CancelSubscriptionCommandHandler 0
>0 1
logger2 8
)8 9
{ 
_context 
= 
context 
; 
_stripe 
= 
stripe 
; 
_logger 
= 
logger 
; 
} 
public 

async 
Task 
< 
Result 
< 
bool !
>! "
>" #
Handle$ *
(* +%
CancelSubscriptionCommand+ D
requestE L
,L M
CancellationTokenN _
ct` b
)b c
{ 
var 
subscription 
= 
await  
_context! )
.) *
Subscriptions* 7
. 
FirstOrDefaultAsync  
(  !
s! "
=># %
s& '
.' (
Id( *
==+ -
request. 5
.5 6
SubscriptionId6 D
,D E
ctF H
)H I
;I J
if   

(   
subscription   
is   
null    
)    !
throw!! 
new!! 
NotFoundException!! '
(!!' (
nameof!!( .
(!!. /
Subscription!!/ ;
)!!; <
,!!< =
request!!> E
.!!E F
SubscriptionId!!F T
)!!T U
;!!U V
if## 

(## 
subscription## 
.## 
UserId## 
!=##  "
request### *
.##* +
UserId##+ 1
)##1 2
throw$$ 
new$$ $
ForbiddenAccessException$$ .
($$. /
)$$/ 0
;$$0 1
if)) 

()) 
!)) 
string)) 
.)) 
IsNullOrWhiteSpace)) &
())& '
subscription))' 3
.))3 4 
StripeSubscriptionId))4 H
)))H I
)))I J
{** 	
try++ 
{,, 
await-- 
_stripe-- 
.-- #
CancelSubscriptionAsync-- 5
(--5 6 
stripeSubscriptionId.. (
:..( )
subscription..* 6
...6 7 
StripeSubscriptionId..7 K
,..K L
cancelImmediately// %
://% &
false//' ,
,//, -
ct00 
:00 
ct00 
)00 
;00 
_logger22 
.22 
LogInformation22 &
(22& '
$str33 L
,33L M
subscription44  
.44  ! 
StripeSubscriptionId44! 5
)445 6
;446 7
}55 
catch66 
(66 
	Exception66 
ex66 
)66  
{77 
_logger;; 
.;; 
LogError;;  
(;;  !
ex;;! #
,;;# $
$str<< f
,<<f g
subscription==  
.==  ! 
StripeSubscriptionId==! 5
)==5 6
;==6 7
}>> 
}?? 	
subscriptionAA 
.AA 

DeactivateAA 
(AA  
)AA  !
;AA! "
awaitBB 
_contextBB 
.BB 
SaveChangesAsyncBB '
(BB' (
ctBB( *
)BB* +
;BB+ ,
returnDD 
ResultDD 
<DD 
boolDD 
>DD 
.DD 
SuccessDD #
(DD# $
trueDD$ (
)DD( )
;DD) *
}EE 
}FF ˇ
êD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subscriptions\Commands\CreateCheckoutSession\CreateCheckoutSessionCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subscriptions' 4
.4 5
Commands5 =
.= >!
CreateCheckoutSession> S
;S T
public 
record (
CreateCheckoutSessionCommand *
(* +
Guid 
UserId	 
, 
SubscriptionType		 
SubscriptionType		 %
)

 
:

 
IRequest

 
<

 
Result

 
<

 #
CheckoutSessionResponse

 +
>

+ ,
>

, -
;

- .
public 
record #
CheckoutSessionResponse %
(% &
string 

	SessionId 
, 
string 

CheckoutUrl 
) 
; Ù5
óD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subscriptions\Commands\CreateCheckoutSession\CreateCheckoutSessionCommandHandler.cs
	namespace		 	
AiTutor		
 
.		 
Application		 
.		 
Features		 &
.		& '
Subscriptions		' 4
.		4 5
Commands		5 =
.		= >!
CreateCheckoutSession		> S
;		S T
public 
class /
#CreateCheckoutSessionCommandHandler 0
: 
IRequestHandler 
< (
CreateCheckoutSessionCommand 2
,2 3
Result4 :
<: ;#
CheckoutSessionResponse; R
>R S
>S T
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
private 
readonly 
IStripeService #
_stripe$ +
;+ ,
public 
/
#CreateCheckoutSessionCommandHandler .
(. /!
IApplicationDbContext 
context %
,% &
IStripeService 
stripe 
) 
{ 
_context 
= 
context 
; 
_stripe 
= 
stripe 
; 
} 
public 

async 
Task 
< 
Result 
< #
CheckoutSessionResponse 4
>4 5
>5 6
Handle7 =
(= >(
CreateCheckoutSessionCommand $
request% ,
,, -
CancellationToken. ?
ct@ B
)B C
{ 
var 
user 
= 
await 
_context !
.! "
Users" '
. 
FirstOrDefaultAsync  
(  !
u! "
=># %
u& '
.' (
Id( *
==+ -
request. 5
.5 6
UserId6 <
,< =
ct> @
)@ A
;A B
if   

(   
user   
is   
null   
)   
throw!! 
new!! 
NotFoundException!! '
(!!' (
nameof!!( .
(!!. /
User!!/ 3
)!!3 4
,!!4 5
request!!6 =
.!!= >
UserId!!> D
)!!D E
;!!E F
var$$ 
hasActiveOrPending$$ 
=$$  
await$$! &
_context$$' /
.$$/ 0
Subscriptions$$0 =
.%% 
AnyAsync%% 
(%% 
s%% 
=>%% 
s&& 
.&& 
UserId&& 
==&& 
request&& #
.&&# $
UserId&&$ *
&&&&+ -
s'' 
.'' 
Type'' 
=='' 
request'' !
.''! "
SubscriptionType''" 2
&&''3 5
((( 
s(( 
.(( 
Status(( 
==(( 
SubscriptionStatus(( /
.((/ 0
Active((0 6
||((7 9
s)) 
.)) 
Status)) 
==)) 
SubscriptionStatus)) /
.))/ 0
Pending))0 7
)))7 8
,))8 9
ct)): <
)))< =
;))= >
if++ 

(++ 
hasActiveOrPending++ 
)++ 
{,, 	
return-- 
Result-- 
<-- #
CheckoutSessionResponse-- 1
>--1 2
.--2 3
Failure--3 :
(--: ;
$str.. S
,..S T
$num// 
)// 
;// 
}00 	
var33 
checkoutResult33 
=33 
await33 "
_stripe33# *
.33* +&
CreateCheckoutSessionAsync33+ E
(33E F
userId44 
:44 
request44 
.44 
UserId44 "
,44" #
	userEmail55 
:55 
user55 
.55 
Email55 !
.55! "
Value55" '
,55' (
subscriptionType66 
:66 
request66 %
.66% &
SubscriptionType66& 6
,666 7
ct77 
:77 
ct77 
)77 
;77 
var:: 
pendingSubscription:: 
=::  !
Subscription::" .
.::. /
CreatePending::/ <
(::< =
userId;; 
:;; 
request;; 
.;; 
UserId;; "
,;;" #
type<< 
:<< 
request<< 
.<< 
SubscriptionType<< *
,<<* +
price== 
:== 
ResolvePriceFor== "
(==" #
request==# *
.==* +
SubscriptionType==+ ;
)==; <
,==< =
stripePriceId>> 
:>> 
checkoutResult>> )
.>>) *
StripePriceId>>* 7
,>>7 8
stripeCustomerId?? 
:?? 
string?? $
.??$ %
IsNullOrWhiteSpace??% 7
(??7 8
checkoutResult??8 F
.??F G
StripeCustomerId??G W
)??W X
?@@ 
null@@ 
:AA 
checkoutResultAA  
.AA  !
StripeCustomerIdAA! 1
)AA1 2
;AA2 3
_contextCC 
.CC 
SubscriptionsCC 
.CC 
AddCC "
(CC" #
pendingSubscriptionCC# 6
)CC6 7
;CC7 8
awaitDD 
_contextDD 
.DD 
SaveChangesAsyncDD '
(DD' (
ctDD( *
)DD* +
;DD+ ,
returnFF 
ResultFF 
<FF #
CheckoutSessionResponseFF -
>FF- .
.FF. /
SuccessFF/ 6
(FF6 7
newGG #
CheckoutSessionResponseGG '
(GG' (
	SessionIdHH 
:HH 
checkoutResultHH )
.HH) *
	SessionIdHH* 3
,HH3 4
CheckoutUrlII 
:II 
checkoutResultII +
.II+ ,
CheckoutUrlII, 7
)II7 8
,II8 9
$numJJ 
)JJ 
;JJ 
}KK 
privateRR 
staticRR 
decimalRR 
ResolvePriceForRR *
(RR* +
SubscriptionTypeRR+ ;
typeRR< @
)RR@ A
=>RRB D
typeRRE I
switchRRJ P
{SS 
SubscriptionTypeTT 
.TT 
ParentMonthlyTT &
=>TT' )
$numTT* 0
,TT0 1
SubscriptionTypeUU 
.UU 
ParentYearlyUU %
=>UU& (
$numUU) 0
,UU0 1
SubscriptionTypeVV 
.VV 
SchoolVV 
=>VV  "
$numVV# *
,VV* +
_WW 	
=>WW
 
$numWW 
}XX 
;XX 
}YY Œ
ôD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subscriptions\Commands\CreateCheckoutSession\CreateCheckoutSessionCommandValidator.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subscriptions' 4
.4 5
Commands5 =
.= >!
CreateCheckoutSession> S
;S T
public 
class 1
%CreateCheckoutSessionCommandValidator 2
: 
AbstractValidator 
< (
CreateCheckoutSessionCommand 4
>4 5
{ 
public		 
1
%CreateCheckoutSessionCommandValidator		 0
(		0 1
)		1 2
{

 
RuleFor 
( 
x 
=> 
x 
. 
UserId 
) 
. 
NotEmpty 
( 
) 
. 
WithMessage 
( 
$str /
)/ 0
;0 1
RuleFor 
( 
x 
=> 
x 
. 
SubscriptionType '
)' (
. 
IsInEnum 
( 
) 
. 
WithMessage 
( 
$str 5
)5 6
. 
NotEqual 
( 
SubscriptionType &
.& '
Free' +
)+ ,
. 
WithMessage 
( 
$str I
)I J
;J K
} 
} ü
äD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subscriptions\Commands\CreateSubscription\CreateSubscriptionCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subscriptions' 4
.4 5
Commands5 =
.= >
CreateSubscription> P
;P Q
public 
record %
CreateSubscriptionCommand '
(' (
Guid 
UserId	 
, 
SubscriptionType		 
Type		 
,		 
decimal

 
Price

 
,

 
DateTime 
	StartDate 
, 
DateTime 
EndDate 
) 
: 
IRequest 
< 
Result 
<  
SubscriptionResponse (
>( )
>) *
;* +
public 
record  
SubscriptionResponse "
(" #
Guid 
Id	 
, 
Guid 
UserId	 
, 
SubscriptionType 
Type 
, 
decimal 
Price 
, 
DateTime 
	StartDate 
, 
DateTime 
EndDate 
, 
bool 
IsActive	 
) 
; ä
ëD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subscriptions\Commands\CreateSubscription\CreateSubscriptionCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subscriptions' 4
.4 5
Commands5 =
.= >
CreateSubscription> P
;P Q
public

 
class

 ,
 CreateSubscriptionCommandHandler

 -
:

. /
IRequestHandler

0 ?
<

? @%
CreateSubscriptionCommand

@ Y
,

Y Z
Result

[ a
<

a b 
SubscriptionResponse

b v
>

v w
>

w x
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
,
 CreateSubscriptionCommandHandler +
(+ ,!
IApplicationDbContext, A
contextB I
)I J
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
<  
SubscriptionResponse 1
>1 2
>2 3
Handle4 :
(: ;%
CreateSubscriptionCommand !
request" )
,) *
CancellationToken+ <
ct= ?
)? @
{ 
var 

userExists 
= 
await 
_context '
.' (
Users( -
. 
AnyAsync 
( 
u 
=> 
u 
. 
Id 
==  "
request# *
.* +
UserId+ 1
,1 2
ct3 5
)5 6
;6 7
if 

( 
! 

userExists 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
User/ 3
)3 4
,4 5
request6 =
.= >
UserId> D
)D E
;E F
var 
subscription 
= 
Subscription '
.' (
Create( .
(. /
request 
. 
UserId 
, 
request 
. 
Type 
, 
request 
. 
	StartDate 
, 
request 
. 
EndDate 
, 
request 
. 
Price 
)   	
;  	 

_context"" 
."" 
Subscriptions"" 
."" 
Add"" "
(""" #
subscription""# /
)""/ 0
;""0 1
await## 
_context## 
.## 
SaveChangesAsync## '
(##' (
ct##( *
)##* +
;##+ ,
return%% 
Result%% 
<%%  
SubscriptionResponse%% *
>%%* +
.%%+ ,
Success%%, 3
(%%3 4
new%%4 7 
SubscriptionResponse%%8 L
(%%L M
subscription&& 
.&& 
Id&& 
,&& 
subscription'' 
.'' 
UserId'' 
,''  
subscription(( 
.(( 
Type(( 
,(( 
subscription)) 
.)) 
Price)) 
,)) 
subscription** 
.** 
	StartDate** "
,**" #
subscription++ 
.++ 
EndDate++  
,++  !
subscription,, 
.,, 
IsValid,,  
(,,  !
),,! "
)-- 	
,--	 

$num-- 
)-- 
;-- 
}.. 
}// ⁄
ìD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subscriptions\Commands\CreateSubscription\CreateSubscriptionCommandValidator.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subscriptions' 4
.4 5
Commands5 =
.= >
CreateSubscription> P
;P Q
public 
class .
"CreateSubscriptionCommandValidator /
:0 1
AbstractValidator2 C
<C D%
CreateSubscriptionCommandD ]
>] ^
{ 
public 
.
"CreateSubscriptionCommandValidator -
(- .
). /
{ 
RuleFor		 
(		 
x		 
=>		 
x		 
.		 
UserId		 
)		 
.		 
NotEmpty		 '
(		' (
)		( )
.		) *
WithMessage		* 5
(		5 6
$str		6 K
)		K L
;		L M
RuleFor

 
(

 
x

 
=>

 
x

 
.

 
Price

 
)

 
.

 
GreaterThan

 )
(

) *
$num

* +
)

+ ,
.

, -
WithMessage

- 8
(

8 9
$str

9 X
)

X Y
;

Y Z
RuleFor 
( 
x 
=> 
x 
. 
	StartDate  
)  !
.! "
LessThan" *
(* +
x+ ,
=>- /
x0 1
.1 2
EndDate2 9
)9 :
.: ;
WithMessage; F
(F G
$strG j
)j k
;k l
RuleFor 
( 
x 
=> 
x 
. 
EndDate 
) 
.  
GreaterThan  +
(+ ,
DateTime, 4
.4 5
UtcNow5 ;
); <
.< =
WithMessage= H
(H I
$strI i
)i j
;j k
} 
} ˝
åD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subscriptions\Commands\HandleStripeWebhook\HandleStripeWebhookCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subscriptions' 4
.4 5
Commands5 =
.= >
HandleStripeWebhook> Q
;Q R
public

 
record

 &
HandleStripeWebhookCommand

 (
(

( )
string 

Payload 
, 
string 

SignatureHeader 
) 
: 
IRequest 
< 
Result 
< 
bool 
> 
> 
; Àã
ìD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subscriptions\Commands\HandleStripeWebhook\HandleStripeWebhookCommandHandler.cs
	namespace

 	
AiTutor


 
.

 
Application

 
.

 
Features

 &
.

& '
Subscriptions

' 4
.

4 5
Commands

5 =
.

= >
HandleStripeWebhook

> Q
;

Q R
public 
class -
!HandleStripeWebhookCommandHandler .
: 
IRequestHandler 
< &
HandleStripeWebhookCommand 0
,0 1
Result2 8
<8 9
bool9 =
>= >
>> ?
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
private 
readonly 
IStripeService #
_stripe$ +
;+ ,
private 
readonly 
ILogger 
< -
!HandleStripeWebhookCommandHandler >
>> ?
_logger@ G
;G H
public 
-
!HandleStripeWebhookCommandHandler ,
(, -!
IApplicationDbContext 
context %
,% &
IStripeService 
stripe 
, 
ILogger 
< -
!HandleStripeWebhookCommandHandler 1
>1 2
logger3 9
)9 :
{ 
_context 
= 
context 
; 
_stripe 
= 
stripe 
; 
_logger 
= 
logger 
; 
} 
public 

async 
Task 
< 
Result 
< 
bool !
>! "
>" #
Handle$ *
(* +&
HandleStripeWebhookCommand "
request# *
,* +
CancellationToken, =
ct> @
)@ A
{ 
StripeWebhookEvent## 
evt## 
;## 
try$$ 
{%% 	
evt&& 
=&& 
_stripe&& 
.&& !
ConstructWebhookEvent&& /
(&&/ 0
request&&0 7
.&&7 8
Payload&&8 ?
,&&? @
request&&A H
.&&H I
SignatureHeader&&I X
)&&X Y
;&&Y Z
}'' 	
catch(( 
((( 
	Exception(( 
ex(( 
)(( 
{)) 	
_logger** 
.** 

LogWarning** 
(** 
ex** !
,**! "
$str**# P
)**P Q
;**Q R
return++ 
Result++ 
<++ 
bool++ 
>++ 
.++  
Failure++  '
(++' (
$str++( D
,++D E
$num++F I
)++I J
;++J K
},, 	
_logger.. 
... 
LogInformation.. 
(.. 
$str// C
,//C D
evt00 
.00 
Id00 
,00 
evt00 
.00 
Type00 
)00 
;00 
switch33 
(33 
evt33 
.33 
Type33 
)33 
{44 	
case55 
$str55 -
:55- .
await66 #
HandleCheckoutCompleted66 -
(66- .
evt66. 1
,661 2
ct663 5
)665 6
;666 7
break77 
;77 
case99 
$str99 0
:990 1
case:: 
$str:: 0
:::0 1
await;; %
HandleSubscriptionUpdated;; /
(;;/ 0
evt;;0 3
,;;3 4
ct;;5 7
);;7 8
;;;8 9
break<< 
;<< 
case>> 
$str>> 0
:>>0 1
await?? %
HandleSubscriptionDeleted?? /
(??/ 0
evt??0 3
,??3 4
ct??5 7
)??7 8
;??8 9
break@@ 
;@@ 
defaultBB 
:BB 
_loggerEE 
.EE 
LogDebugEE  
(EE  !
$strEE! D
,EED E
evtEEF I
.EEI J
TypeEEJ N
)EEN O
;EEO P
breakFF 
;FF 
}GG 	
returnII 
ResultII 
<II 
boolII 
>II 
.II 
SuccessII #
(II# $
trueII$ (
)II( )
;II) *
}JJ 
privateNN 
asyncNN 
TaskNN #
HandleCheckoutCompletedNN .
(NN. /
StripeWebhookEventNN/ A
evtNNB E
,NNE F
CancellationTokenNNG X
ctNNY [
)NN[ \
{OO 
ifPP 

(PP 
evtPP 
.PP 
CheckoutSessionPP 
isPP  "
nullPP# '
)PP' (
{QQ 	
_loggerRR 
.RR 

LogWarningRR 
(RR 
$strRR \
)RR\ ]
;RR] ^
returnSS 
;SS 
}TT 	
varVV 
dataVV 
=VV 
evtVV 
.VV 
CheckoutSessionVV &
;VV& '
ifXX 

(XX 
dataXX 
.XX 
	AppUserIdXX 
isXX 
nullXX "
)XX" #
{YY 	
_loggerZZ 
.ZZ 

LogWarningZZ 
(ZZ 
$str[[ e
,[[e f
data\\ 
.\\ 
	SessionId\\ 
)\\ 
;\\  
return]] 
;]] 
}^^ 	
varcc 
pendingcc 
=cc 
awaitcc 
_contextcc $
.cc$ %
Subscriptionscc% 2
.dd 
Wheredd 
(dd 
sdd 
=>dd 
see 
.ee 
UserIdee 
==ee 
dataee  
.ee  !
	AppUserIdee! *
.ee* +
Valueee+ 0
&&ee1 3
sff 
.ff 
Statusff 
==ff 
SubscriptionStatusff .
.ff. /
Pendingff/ 6
)ff6 7
.gg 
OrderByDescendinggg 
(gg 
sgg  
=>gg! #
sgg$ %
.gg% &
	CreatedAtgg& /
)gg/ 0
.hh 
FirstOrDefaultAsynchh  
(hh  !
cthh! #
)hh# $
;hh$ %
ifjj 

(jj 
pendingjj 
isjj 
nulljj 
)jj 
{kk 	
_loggerll 
.ll 

LogWarningll 
(ll 
$strmm R
,mmR S
datann 
.nn 
	AppUserIdnn 
,nn 
datann  $
.nn$ %
	SessionIdnn% .
)nn. /
;nn/ 0
returnoo 
;oo 
}pp 	
ifss 

(ss 
pendingss 
.ss 
HasProcessedEventss %
(ss% &
evtss& )
.ss) *
Idss* ,
)ss, -
)ss- .
{tt 	
_loggeruu 
.uu 
LogInformationuu "
(uu" #
$struu# O
,uuO P
evtuuQ T
.uuT U
IduuU W
)uuW X
;uuX Y
returnvv 
;vv 
}ww 	
ifzz 

(zz 
!zz 
stringzz 
.zz 
IsNullOrWhiteSpacezz &
(zz& '
datazz' +
.zz+ ,
SubscriptionIdzz, :
)zz: ;
&&zz< >
!{{ 
string{{ 
.{{ 
IsNullOrWhiteSpace{{ &
({{& '
data{{' +
.{{+ ,

CustomerId{{, 6
){{6 7
){{7 8
{|| 	
pending}} 
.}} 

LinkStripe}} 
(}} 
data}} #
.}}# $

CustomerId}}$ .
,}}. /
data}}0 4
.}}4 5
SubscriptionId}}5 C
)}}C D
;}}D E
}~~ 	
pending
ÄÄ 
.
ÄÄ 
RecordStripeEvent
ÄÄ !
(
ÄÄ! "
evt
ÄÄ" %
.
ÄÄ% &
Id
ÄÄ& (
)
ÄÄ( )
;
ÄÄ) *
await
ÅÅ 
_context
ÅÅ 
.
ÅÅ 
SaveChangesAsync
ÅÅ '
(
ÅÅ' (
ct
ÅÅ( *
)
ÅÅ* +
;
ÅÅ+ ,
_logger
ÉÉ 
.
ÉÉ 
LogInformation
ÉÉ 
(
ÉÉ 
$str
ÑÑ Q
,
ÑÑQ R
pending
ÖÖ 
.
ÖÖ 
Id
ÖÖ 
,
ÖÖ 
data
ÖÖ 
.
ÖÖ 

CustomerId
ÖÖ '
,
ÖÖ' (
data
ÖÖ) -
.
ÖÖ- .
SubscriptionId
ÖÖ. <
)
ÖÖ< =
;
ÖÖ= >
}
ââ 
private
ãã 
async
ãã 
Task
ãã '
HandleSubscriptionUpdated
ãã 0
(
ãã0 1 
StripeWebhookEvent
ãã1 C
evt
ããD G
,
ããG H
CancellationToken
ããI Z
ct
ãã[ ]
)
ãã] ^
{
åå 
if
çç 

(
çç 
evt
çç 
.
çç 
Subscription
çç 
is
çç 
null
çç  $
)
çç$ %
{
éé 	
_logger
èè 
.
èè 

LogWarning
èè 
(
èè 
$str
èè [
)
èè[ \
;
èè\ ]
return
êê 
;
êê 
}
ëë 	
var
ìì 
data
ìì 
=
ìì 
evt
ìì 
.
ìì 
Subscription
ìì #
;
ìì# $
var
ïï 
subscription
ïï 
=
ïï 
await
ïï  
FindSubscription
ïï! 1
(
ïï1 2
data
ïï2 6
,
ïï6 7
ct
ïï8 :
)
ïï: ;
;
ïï; <
if
ññ 

(
ññ 
subscription
ññ 
is
ññ 
null
ññ  
)
ññ  !
{
óó 	
_logger
òò 
.
òò 

LogWarning
òò 
(
òò 
$str
ôô B
,
ôôB C
data
öö 
.
öö 
SubscriptionId
öö #
)
öö# $
;
öö$ %
return
õõ 
;
õõ 
}
úú 	
if
ûû 

(
ûû 
subscription
ûû 
.
ûû 
HasProcessedEvent
ûû *
(
ûû* +
evt
ûû+ .
.
ûû. /
Id
ûû/ 1
)
ûû1 2
)
ûû2 3
{
üü 	
_logger
†† 
.
†† 
LogInformation
†† "
(
††" #
$str
††# O
,
††O P
evt
††Q T
.
††T U
Id
††U W
)
††W X
;
††X Y
return
°° 
;
°° 
}
¢¢ 	
if
¶¶ 

(
¶¶ 
string
¶¶ 
.
¶¶  
IsNullOrWhiteSpace
¶¶ %
(
¶¶% &
subscription
¶¶& 2
.
¶¶2 3"
StripeSubscriptionId
¶¶3 G
)
¶¶G H
)
¶¶H I
{
ßß 	
subscription
®® 
.
®® 

LinkStripe
®® #
(
®®# $
data
®®$ (
.
®®( )

CustomerId
®®) 3
,
®®3 4
data
®®5 9
.
®®9 :
SubscriptionId
®®: H
)
®®H I
;
®®I J
}
©© 	
var
¨¨ 
domainStatus
¨¨ 
=
¨¨  
StripeStatusMapper
¨¨ -
.
¨¨- .
ToDomainStatus
¨¨. <
(
¨¨< =
data
¨¨= A
.
¨¨A B
Status
¨¨B H
)
¨¨H I
;
¨¨I J
switch
ÆÆ 
(
ÆÆ 
domainStatus
ÆÆ 
)
ÆÆ 
{
ØØ 	
case
∞∞  
SubscriptionStatus
∞∞ #
.
∞∞# $
Active
∞∞$ *
:
∞∞* +
subscription
±± 
.
±± 

MarkActive
±± '
(
±±' (
data
±±( ,
.
±±, - 
CurrentPeriodStart
±±- ?
,
±±? @
data
±±A E
.
±±E F
CurrentPeriodEnd
±±F V
)
±±V W
;
±±W X
break
≤≤ 
;
≤≤ 
case
¥¥  
SubscriptionStatus
¥¥ #
.
¥¥# $
PastDue
¥¥$ +
:
¥¥+ ,
subscription
µµ 
.
µµ 
MarkPastDue
µµ (
(
µµ( )
)
µµ) *
;
µµ* +
break
∂∂ 
;
∂∂ 
case
∏∏  
SubscriptionStatus
∏∏ #
.
∏∏# $

Incomplete
∏∏$ .
:
∏∏. /
subscription
ππ 
.
ππ 
MarkIncomplete
ππ +
(
ππ+ ,
)
ππ, -
;
ππ- .
break
∫∫ 
;
∫∫ 
case
ºº  
SubscriptionStatus
ºº #
.
ºº# $
Canceled
ºº$ ,
:
ºº, -
subscription
ΩΩ 
.
ΩΩ "
MarkCanceledByStripe
ΩΩ 1
(
ΩΩ1 2
)
ΩΩ2 3
;
ΩΩ3 4
break
ææ 
;
ææ 
default
¿¿ 
:
¿¿ 
_logger
¡¡ 
.
¡¡ 
LogDebug
¡¡  
(
¡¡  !
$str
¬¬ J
,
¬¬J K
data
√√ 
.
√√ 
Status
√√ 
)
√√  
;
√√  !
break
ƒƒ 
;
ƒƒ 
}
≈≈ 	
subscription
«« 
.
«« 
RecordStripeEvent
«« &
(
««& '
evt
««' *
.
««* +
Id
««+ -
)
««- .
;
««. /
await
»» 
_context
»» 
.
»» 
SaveChangesAsync
»» '
(
»»' (
ct
»»( *
)
»»* +
;
»»+ ,
_logger
   
.
   
LogInformation
   
(
   
$str
ÀÀ S
,
ÀÀS T
subscription
ÃÃ 
.
ÃÃ 
Id
ÃÃ 
,
ÃÃ 
domainStatus
ÃÃ )
,
ÃÃ) *
evt
ÃÃ+ .
.
ÃÃ. /
Id
ÃÃ/ 1
)
ÃÃ1 2
;
ÃÃ2 3
}
ÕÕ 
private
œœ 
async
œœ 
Task
œœ '
HandleSubscriptionDeleted
œœ 0
(
œœ0 1 
StripeWebhookEvent
œœ1 C
evt
œœD G
,
œœG H
CancellationToken
œœI Z
ct
œœ[ ]
)
œœ] ^
{
–– 
if
—— 

(
—— 
evt
—— 
.
—— 
Subscription
—— 
is
—— 
null
——  $
)
——$ %
return
——& ,
;
——, -
var
”” 
subscription
”” 
=
”” 
await
””  
FindSubscription
””! 1
(
””1 2
evt
””2 5
.
””5 6
Subscription
””6 B
,
””B C
ct
””D F
)
””F G
;
””G H
if
‘‘ 

(
‘‘ 
subscription
‘‘ 
is
‘‘ 
null
‘‘  
)
‘‘  !
return
‘‘" (
;
‘‘( )
if
÷÷ 

(
÷÷ 
subscription
÷÷ 
.
÷÷ 
HasProcessedEvent
÷÷ *
(
÷÷* +
evt
÷÷+ .
.
÷÷. /
Id
÷÷/ 1
)
÷÷1 2
)
÷÷2 3
return
÷÷4 :
;
÷÷: ;
subscription
ÿÿ 
.
ÿÿ "
MarkCanceledByStripe
ÿÿ )
(
ÿÿ) *
)
ÿÿ* +
;
ÿÿ+ ,
subscription
ŸŸ 
.
ŸŸ 
RecordStripeEvent
ŸŸ &
(
ŸŸ& '
evt
ŸŸ' *
.
ŸŸ* +
Id
ŸŸ+ -
)
ŸŸ- .
;
ŸŸ. /
await
⁄⁄ 
_context
⁄⁄ 
.
⁄⁄ 
SaveChangesAsync
⁄⁄ '
(
⁄⁄' (
ct
⁄⁄( *
)
⁄⁄* +
;
⁄⁄+ ,
_logger
‹‹ 
.
‹‹ 
LogInformation
‹‹ 
(
‹‹ 
$str
›› G
,
››G H
subscription
ﬁﬁ 
.
ﬁﬁ 
Id
ﬁﬁ 
,
ﬁﬁ 
evt
ﬁﬁ  
.
ﬁﬁ  !
Id
ﬁﬁ! #
)
ﬁﬁ# $
;
ﬁﬁ$ %
}
ﬂﬂ 
private
ÈÈ 
async
ÈÈ 
Task
ÈÈ 
<
ÈÈ 
Subscription
ÈÈ #
?
ÈÈ# $
>
ÈÈ$ %
FindSubscription
ÈÈ& 6
(
ÈÈ6 7$
StripeSubscriptionData
ÍÍ 
data
ÍÍ #
,
ÍÍ# $
CancellationToken
ÍÍ% 6
ct
ÍÍ7 9
)
ÍÍ9 :
{
ÎÎ 
var
ÌÌ 
matched
ÌÌ 
=
ÌÌ 
await
ÌÌ 
_context
ÌÌ $
.
ÌÌ$ %
Subscriptions
ÌÌ% 2
.
ÓÓ !
FirstOrDefaultAsync
ÓÓ  
(
ÓÓ  !
s
ÓÓ! "
=>
ÓÓ# %
s
ÓÓ& '
.
ÓÓ' ("
StripeSubscriptionId
ÓÓ( <
==
ÓÓ= ?
data
ÓÓ@ D
.
ÓÓD E
SubscriptionId
ÓÓE S
,
ÓÓS T
ct
ÓÓU W
)
ÓÓW X
;
ÓÓX Y
if
 

(
 
matched
 
is
 
not
 
null
 
)
  
return
! '
matched
( /
;
/ 0
if
ÛÛ 

(
ÛÛ 
data
ÛÛ 
.
ÛÛ 
	AppUserId
ÛÛ 
is
ÛÛ 
null
ÛÛ "
)
ÛÛ" #
return
ÛÛ$ *
null
ÛÛ+ /
;
ÛÛ/ 0
return
ıı 
await
ıı 
_context
ıı 
.
ıı 
Subscriptions
ıı +
.
ˆˆ 
Where
ˆˆ 
(
ˆˆ 
s
ˆˆ 
=>
ˆˆ 
s
˜˜ 
.
˜˜ 
UserId
˜˜ 
==
˜˜ 
data
˜˜  
.
˜˜  !
	AppUserId
˜˜! *
.
˜˜* +
Value
˜˜+ 0
&&
˜˜1 3
s
¯¯ 
.
¯¯ 
Status
¯¯ 
==
¯¯  
SubscriptionStatus
¯¯ .
.
¯¯. /
Pending
¯¯/ 6
)
¯¯6 7
.
˘˘ 
OrderByDescending
˘˘ 
(
˘˘ 
s
˘˘  
=>
˘˘! #
s
˘˘$ %
.
˘˘% &
	CreatedAt
˘˘& /
)
˘˘/ 0
.
˙˙ !
FirstOrDefaultAsync
˙˙  
(
˙˙  !
ct
˙˙! #
)
˙˙# $
;
˙˙$ %
}
˚˚ 
}¸¸ Ù
âD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subscriptions\Queries\GetAllSubscriptions\GetAllSubscriptionsQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subscriptions' 4
.4 5
Queries5 <
.< =
GetAllSubscriptions= P
;P Q
public 
record $
GetAllSubscriptionsQuery &
(& '
int' *
Page+ /
=0 1
$num2 3
,3 4
int5 8
PageSize9 A
=B C
$numD F
)F G
: 
IRequest 
< 
Result 
< 
PaginatedList #
<# $ 
SubscriptionResponse$ 8
>8 9
>9 :
>: ;
;; <í
êD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subscriptions\Queries\GetAllSubscriptions\GetAllSubscriptionsQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subscriptions' 4
.4 5
Queries5 <
.< =
GetAllSubscriptions= P
;P Q
public		 
class		 +
GetAllSubscriptionsQueryHandler		 ,
:

 
IRequestHandler

 
<

 $
GetAllSubscriptionsQuery

 .
,

. /
Result

0 6
<

6 7
PaginatedList

7 D
<

D E 
SubscriptionResponse

E Y
>

Y Z
>

Z [
>

[ \
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
+
GetAllSubscriptionsQueryHandler *
(* +!
IApplicationDbContext+ @
contextA H
)H I
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
PaginatedList *
<* + 
SubscriptionResponse+ ?
>? @
>@ A
>A B
HandleC I
(I J$
GetAllSubscriptionsQuery  
request! (
,( )
CancellationToken* ;
ct< >
)> ?
{ 
var 
query 
= 
_context 
. 
Subscriptions *
. 
OrderByDescending 
( 
s  
=>! #
s$ %
.% &
	StartDate& /
)/ 0
;0 1
var 
total 
= 
await 
query 
.  

CountAsync  *
(* +
ct+ -
)- .
;. /
var 
items 
= 
await 
query 
. 
Skip 
( 
( 
request 
. 
Page 
-  !
$num" #
)# $
*% &
request' .
.. /
PageSize/ 7
)7 8
. 
Take 
( 
request 
. 
PageSize "
)" #
. 
ToListAsync 
( 
ct 
) 
; 
var 
response 
= 
items 
. 
Select #
(# $
s$ %
=>& (
new) , 
SubscriptionResponse- A
(A B
s 
. 
Id 
, 
s   
.   
UserId   
,   
s!! 
.!! 
Type!! 
,!! 
s"" 
."" 
Price"" 
,"" 
s## 
.## 
	StartDate## 
,## 
s$$ 
.$$ 
EndDate$$ 
,$$ 
s%% 
.%% 
IsValid%% 
(%% 
)%% 
)&& 	
)&&	 

.&&
 
ToList&& 
(&& 
)&& 
;&& 
return(( 
Result(( 
<(( 
PaginatedList(( #
<((# $ 
SubscriptionResponse(($ 8
>((8 9
>((9 :
.((: ;
Success((; B
(((B C
new)) 
PaginatedList)) 
<))  
SubscriptionResponse)) 2
>))2 3
())3 4
response))4 <
,))< =
total))> C
,))C D
request))E L
.))L M
Page))M Q
,))Q R
request))S Z
.))Z [
PageSize))[ c
)))c d
)))d e
;))e f
}** 
}++ ˛
çD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subscriptions\Queries\GetSubscriptionByUser\GetSubscriptionByUserQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subscriptions' 4
.4 5
Queries5 <
.< =!
GetSubscriptionByUser= R
;R S
public 
record &
GetSubscriptionByUserQuery (
(( )
Guid) -
UserId. 4
)4 5
: 
IRequest 
< 
Result 
< 
List 
<  
SubscriptionResponse /
>/ 0
>0 1
>1 2
;2 3¨
îD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Subscriptions\Queries\GetSubscriptionByUser\GetSubscriptionByUserQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Subscriptions' 4
.4 5
Queries5 <
.< =!
GetSubscriptionByUser= R
;R S
public		 
class		 -
!GetSubscriptionByUserQueryHandler		 .
:

 
IRequestHandler

 
<

 &
GetSubscriptionByUserQuery

 0
,

0 1
Result

2 8
<

8 9
List

9 =
<

= > 
SubscriptionResponse

> R
>

R S
>

S T
>

T U
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
-
!GetSubscriptionByUserQueryHandler ,
(, -!
IApplicationDbContext- B
contextC J
)J K
=> 

_context 
= 
context 
; 
public 

async 
Task 
< 
Result 
< 
List !
<! " 
SubscriptionResponse" 6
>6 7
>7 8
>8 9
Handle: @
(@ A&
GetSubscriptionByUserQuery "
request# *
,* +
CancellationToken, =
ct> @
)@ A
{ 
var 
subscriptions 
= 
await !
_context" *
.* +
Subscriptions+ 8
. 
Where 
( 
s 
=> 
s 
. 
UserId  
==! #
request$ +
.+ ,
UserId, 2
)2 3
. 
OrderByDescending 
( 
s  
=>! #
s$ %
.% &
	StartDate& /
)/ 0
. 
ToListAsync 
( 
ct 
) 
; 
var 
response 
= 
subscriptions $
.$ %
Select% +
(+ ,
s, -
=>. 0
new1 4 
SubscriptionResponse5 I
(I J
s 
. 
Id 
, 
s 
. 
UserId 
, 
s 
. 
Type 
, 
s 
. 
Price 
, 
s 
. 
	StartDate 
, 
s 
. 
EndDate 
, 
s   
.   
IsValid   
(   
)   
)!! 	
)!!	 

.!!
 
ToList!! 
(!! 
)!! 
;!! 
return## 
Result## 
<## 
List## 
<##  
SubscriptionResponse## /
>##/ 0
>##0 1
.##1 2
Success##2 9
(##9 :
response##: B
)##B C
;##C D
}$$ 
}%% ¥	
rD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Commands\CreateUser\CreateUserCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Commands- 5
.5 6

CreateUser6 @
;@ A
public 
record 
CreateUserCommand 
(  
string 

	FirstName 
, 
string		 

LastName		 
,		 
string

 

Email

 
,

 
string 

Password 
, 
UserRole 
Role 
) 
: 
IRequest 
< 
Result $
<$ %
CreateUserResponse% 7
>7 8
>8 9
;9 :
public 
record 
CreateUserResponse  
(  !
Guid! %
Id& (
,( )
string* 0
FullName1 9
,9 :
string; A
EmailB G
,G H
UserRoleI Q
RoleR V
)V W
;W X£
yD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Commands\CreateUser\CreateUserCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Commands- 5
.5 6

CreateUser6 @
;@ A
public 
class $
CreateUserCommandHandler %
:& '
IRequestHandler( 7
<7 8
CreateUserCommand8 I
,I J
ResultK Q
<Q R
CreateUserResponseR d
>d e
>e f
{		 
private

 
readonly

 !
IApplicationDbContext

 *
_context

+ 3
;

3 4
public 
$
CreateUserCommandHandler #
(# $!
IApplicationDbContext$ 9
context: A
)A B
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
CreateUserResponse /
>/ 0
>0 1
Handle2 8
(8 9
CreateUserCommand9 J
requestK R
,R S
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
user 
= 
User 
. 
Create 
( 
request &
.& '
	FirstName' 0
,0 1
request2 9
.9 :
LastName: B
,B C
requestD K
.K L
EmailL Q
,Q R
requestS Z
.Z [
Role[ _
)_ `
;` a
_context 
. 
Users 
. 
Add 
( 
user 
)  
;  !
await 
_context 
. 
SaveChangesAsync '
(' (
cancellationToken( 9
)9 :
;: ;
return 
Result 
< 
CreateUserResponse (
>( )
.) *
Success* 1
(1 2
new 
CreateUserResponse "
(" #
user# '
.' (
Id( *
,* +
user, 0
.0 1
FullName1 9
,9 :
user; ?
.? @
Email@ E
.E F
ValueF K
,K L
userM Q
.Q R
RoleR V
)V W
,W X
$numY \
)\ ]
;] ^
} 
} Ü
{D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Commands\CreateUser\CreateUserCommandValidator.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Commands- 5
.5 6

CreateUser6 @
;@ A
public 
class &
CreateUserCommandValidator '
:( )
AbstractValidator* ;
<; <
CreateUserCommand< M
>M N
{ 
public 
&
CreateUserCommandValidator %
(% &
)& '
{ 
RuleFor		 
(		 
x		 
=>		 
x		 
.		 
	FirstName		  
)		  !
.

 
NotEmpty

 
(

 
)

 
.

 
WithMessage

 #
(

# $
$str

$ =
)

= >
. 
MaximumLength 
( 
$num 
) 
.  
WithMessage  +
(+ ,
$str, X
)X Y
;Y Z
RuleFor 
( 
x 
=> 
x 
. 
LastName 
)  
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ <
)< =
. 
MaximumLength 
( 
$num 
) 
.  
WithMessage  +
(+ ,
$str, W
)W X
;X Y
RuleFor 
( 
x 
=> 
x 
. 
Email 
) 
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ 8
)8 9
. 
EmailAddress 
( 
) 
. 
WithMessage '
(' (
$str( =
)= >
;> ?
RuleFor 
( 
x 
=> 
x 
. 
Password 
)  
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ ;
); <
. 
MinimumLength 
( 
$num 
) 
. 
WithMessage )
() *
$str* S
)S T
. 
Matches 
( 
$str 
) 
. 
WithMessage )
() *
$str* `
)` a
. 
Matches 
( 
$str 
) 
. 
WithMessage )
() *
$str* V
)V W
;W X
} 
} ±
rD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Commands\LinkParent\LinkParentCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Commands- 5
.5 6

LinkParent6 @
;@ A
public 
record 
LinkParentCommand 
(  
Guid  $
	StudentId% .
,. /
string0 6
InvitationCode7 E
)E F
: 
IRequest 
< 
Result 
< 
LinkParentResponse (
>( )
>) *
;* +
public		 
record		 
LinkParentResponse		  
(		  !
Guid		! %
ParentId		& .
,		. /
string		0 6
ParentFullName		7 E
)		E F
;		F GÔ%
yD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Commands\LinkParent\LinkParentCommandHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Commands- 5
.5 6

LinkParent6 @
;@ A
public		 
class		 $
LinkParentCommandHandler		 %
:		& '
IRequestHandler		( 7
<		7 8
LinkParentCommand		8 I
,		I J
Result		K Q
<		Q R
LinkParentResponse		R d
>		d e
>		e f
{

 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
$
LinkParentCommandHandler #
(# $!
IApplicationDbContext$ 9
context: A
)A B
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
LinkParentResponse /
>/ 0
>0 1
Handle2 8
(8 9
LinkParentCommand9 J
requestK R
,R S
CancellationTokenT e
cancellationTokenf w
)w x
{ 
var 
student 
= 
await 
_context $
.$ %
Users% *
. 
FirstOrDefaultAsync  
(  !
u! "
=># %
u& '
.' (
Id( *
==+ -
request. 5
.5 6
	StudentId6 ?
,? @
cancellationTokenA R
)R S
;S T
if 

( 
student 
is 
null 
) 
return 
Result 
< 
LinkParentResponse ,
>, -
.- .
Failure. 5
(5 6
$str6 R
,R S
$numT W
)W X
;X Y
if 

( 
student 
. 
Role 
!= 
UserRole $
.$ %
Student% ,
), -
return 
Result 
< 
LinkParentResponse ,
>, -
.- .
Failure. 5
(5 6
$str6 `
,` a
$numb e
)e f
;f g
var 
code 
= 
request 
. 
InvitationCode )
?) *
.* +
Trim+ /
(/ 0
)0 1
.1 2
ToUpperInvariant2 B
(B C
)C D
;D E
if 

( 
string 
. 
IsNullOrWhiteSpace %
(% &
code& *
)* +
)+ ,
return 
Result 
< 
LinkParentResponse ,
>, -
.- .
Failure. 5
(5 6
$str6 \
,\ ]
$num^ a
)a b
;b c
var!! 
parent!! 
=!! 
await!! 
_context!! #
.!!# $
Users!!$ )
."" 
FirstOrDefaultAsync""  
(""  !
u""! "
=>""# %
u""& '
.""' (
InvitationCode""( 6
==""7 9
code"": >
&&""? A
u""B C
.""C D
Role""D H
==""I K
UserRole""L T
.""T U
Parent""U [
,""[ \
cancellationToken""] n
)""n o
;""o p
if$$ 

($$ 
parent$$ 
is$$ 
null$$ 
)$$ 
return%% 
Result%% 
<%% 
LinkParentResponse%% ,
>%%, -
.%%- .
Failure%%. 5
(%%5 6
$str%%6 Q
,%%Q R
$num%%S V
)%%V W
;%%W X
student'' 
.'' 
LinkToParent'' 
('' 
parent'' #
.''# $
Id''$ &
)''& '
;''' (
await(( 
_context(( 
.(( 
SaveChangesAsync(( '
(((' (
cancellationToken((( 9
)((9 :
;((: ;
return** 
Result** 
<** 
LinkParentResponse** (
>**( )
.**) *
Success*** 1
(**1 2
new++ 
LinkParentResponse++ "
(++" #
parent++# )
.++) *
Id++* ,
,++, -
parent++. 4
.++4 5
FullName++5 =
)++= >
,++> ?
$num++@ C
)++C D
;++D E
},, 
}-- ±
vD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Commands\UnlinkParent\UnlinkParentCommand.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Commands- 5
.5 6
UnlinkParent6 B
;B C
public		 
record		 
UnlinkParentCommand		 !
(		! "
Guid		" &
	StudentId		' 0
)		0 1
:		2 3
IRequest		4 <
<		< =
Result		= C
<		C D
bool		D H
>		H I
>		I J
;		J K
public 
class &
UnlinkParentCommandHandler '
:( )
IRequestHandler* 9
<9 :
UnlinkParentCommand: M
,M N
ResultO U
<U V
boolV Z
>Z [
>[ \
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
&
UnlinkParentCommandHandler %
(% &!
IApplicationDbContext& ;
context< C
)C D
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
bool !
>! "
>" #
Handle$ *
(* +
UnlinkParentCommand+ >
request? F
,F G
CancellationTokenH Y
cancellationTokenZ k
)k l
{ 
var 
student 
= 
await 
_context $
.$ %
Users% *
. 
FirstOrDefaultAsync  
(  !
u! "
=># %
u& '
.' (
Id( *
==+ -
request. 5
.5 6
	StudentId6 ?
,? @
cancellationTokenA R
)R S
;S T
if 

( 
student 
is 
null 
) 
return 
Result 
< 
bool 
> 
.  
Failure  '
(' (
$str( D
,D E
$numF I
)I J
;J K
if 

( 
student 
. 
Role 
!= 
UserRole $
.$ %
Student% ,
), -
return 
Result 
< 
bool 
> 
.  
Failure  '
(' (
$str( I
,I J
$numK N
)N O
;O P
if 

( 
student 
. 
ParentId 
is 
null  $
)$ %
return   
Result   
<   
bool   
>   
.    
Failure    '
(  ' (
$str  ( M
,  M N
$num  O R
)  R S
;  S T
student"" 
."" 
UnlinkFromParent""  
(""  !
)""! "
;""" #
await## 
_context## 
.## 
SaveChangesAsync## '
(##' (
cancellationToken##( 9
)##9 :
;##: ;
return%% 
Result%% 
<%% 
bool%% 
>%% 
.%% 
Success%% #
(%%# $
true%%$ (
)%%( )
;%%) *
}&& 
}'' º
qD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Queries\GetAllUsers\GetAllUsersQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Queries- 4
.4 5
GetAllUsers5 @
;@ A
public 
record 
GetAllUsersQuery 
( 
int "

PageNumber# -
=. /
$num0 1
,1 2
int3 6
PageSize7 ?
=@ A
$numB D
)D E
: 
IRequest 
< 
Result 
< 
PaginatedList #
<# $
UserDto$ +
>+ ,
>, -
>- .
;. /°
xD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Queries\GetAllUsers\GetAllUsersQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Queries- 4
.4 5
GetAllUsers5 @
;@ A
public		 
class		 #
GetAllUsersQueryHandler		 $
:		% &
IRequestHandler		' 6
<		6 7
GetAllUsersQuery		7 G
,		G H
Result		I O
<		O P
PaginatedList		P ]
<		] ^
UserDto		^ e
>		e f
>		f g
>		g h
{

 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
#
GetAllUsersQueryHandler "
(" #!
IApplicationDbContext# 8
context9 @
)@ A
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
PaginatedList *
<* +
UserDto+ 2
>2 3
>3 4
>4 5
Handle6 <
(< =
GetAllUsersQuery= M
requestN U
,U V
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
query 
= 
_context 
. 
Users "
." #
AsNoTracking# /
(/ 0
)0 1
;1 2
var 

totalCount 
= 
await 
query $
.$ %

CountAsync% /
(/ 0
cancellationToken0 A
)A B
;B C
var 
users 
= 
await 
query 
. 
Skip 
( 
( 
request 
. 

PageNumber %
-& '
$num( )
)) *
*+ ,
request- 4
.4 5
PageSize5 =
)= >
. 
Take 
( 
request 
. 
PageSize "
)" #
. 
Select 
( 
u 
=> 
new 
UserDto $
($ %
u% &
.& '
Id' )
,) *
u+ ,
., -
FullName- 5
,5 6
u7 8
.8 9
Email9 >
.> ?
Value? D
,D E
uF G
.G H
RoleH L
,L M
uN O
.O P
IsActiveP X
,X Y
uZ [
.[ \
	CreatedAt\ e
)e f
)f g
. 
ToListAsync 
( 
cancellationToken *
)* +
;+ ,
return 
Result 
< 
PaginatedList #
<# $
UserDto$ +
>+ ,
>, -
.- .
Success. 5
(5 6
new   
PaginatedList   
<   
UserDto   %
>  % &
(  & '
users  ' ,
,  , -

totalCount  . 8
,  8 9
request  : A
.  A B

PageNumber  B L
,  L M
request  N U
.  U V
PageSize  V ^
)  ^ _
)  _ `
;  ` a
}!! 
}"" ®
qD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Queries\GetChildren\GetChildrenQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Queries- 4
.4 5
GetChildren5 @
;@ A
public 
record 
GetChildrenQuery 
( 
Guid #
ParentId$ ,
), -
:. /
IRequest0 8
<8 9
Result9 ?
<? @
List@ D
<D E
ChildDtoE M
>M N
>N O
>O P
;P Q
public		 
record		 
ChildDto		 
(		 
Guid

 
Id

	 
,

 
string 

FullName 
, 
string 

Email 
, 
UserRole 
Role 
, 
DateTime 
	CreatedAt 
) 
; Ñ
xD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Queries\GetChildren\GetChildrenQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Queries- 4
.4 5
GetChildren5 @
;@ A
public 
class #
GetChildrenQueryHandler $
:% &
IRequestHandler' 6
<6 7
GetChildrenQuery7 G
,G H
ResultI O
<O P
ListP T
<T U
ChildDtoU ]
>] ^
>^ _
>_ `
{		 
private

 
readonly

 !
IApplicationDbContext

 *
_context

+ 3
;

3 4
public 
#
GetChildrenQueryHandler "
(" #!
IApplicationDbContext# 8
context9 @
)@ A
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
List !
<! "
ChildDto" *
>* +
>+ ,
>, -
Handle. 4
(4 5
GetChildrenQuery5 E
requestF M
,M N
CancellationTokenO `
cancellationTokena r
)r s
{ 
var 
children 
= 
await 
_context %
.% &
Users& +
. 
Where 
( 
u 
=> 
u 
. 
ParentId "
==# %
request& -
.- .
ParentId. 6
)6 7
. 
OrderBy 
( 
u 
=> 
u 
. 
	FirstName %
)% &
. 
Select 
( 
u 
=> 
new 
ChildDto %
(% &
u 
. 
Id 
, 
u 
. 
	FirstName 
+ 
$str !
+" #
u$ %
.% &
LastName& .
,. /
u 
. 
Email 
. 
Value 
, 
u 
. 
Role 
, 
u 
. 
	CreatedAt 
) 
) 
. 
ToListAsync 
( 
cancellationToken *
)* +
;+ ,
return 
Result 
< 
List 
< 
ChildDto #
># $
>$ %
.% &
Success& -
(- .
children. 6
)6 7
;7 8
} 
}   Ö
}D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Queries\GetInvitationCode\GetInvitationCodeQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Queries- 4
.4 5
GetInvitationCode5 F
;F G
public 
record "
GetInvitationCodeQuery $
($ %
Guid% )
ParentId* 2
)2 3
:4 5
IRequest6 >
<> ?
Result? E
<E F
InvitationCodeDtoF W
>W X
>X Y
;Y Z
public 
record 
InvitationCodeDto 
(  
Guid  $
ParentId% -
,- .
string/ 5
InvitationCode6 D
)D E
;E Fº
ÑD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Queries\GetInvitationCode\GetInvitationCodeQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Queries- 4
.4 5
GetInvitationCode5 F
;F G
public		 
class		 )
GetInvitationCodeQueryHandler		 *
:		+ ,
IRequestHandler		- <
<		< ="
GetInvitationCodeQuery		= S
,		S T
Result		U [
<		[ \
InvitationCodeDto		\ m
>		m n
>		n o
{

 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
)
GetInvitationCodeQueryHandler (
(( )!
IApplicationDbContext) >
context? F
)F G
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
InvitationCodeDto .
>. /
>/ 0
Handle1 7
(7 8"
GetInvitationCodeQuery8 N
requestO V
,V W
CancellationTokenX i
cancellationTokenj {
){ |
{ 
var 
parent 
= 
await 
_context #
.# $
Users$ )
. 
FirstOrDefaultAsync  
(  !
u! "
=># %
u& '
.' (
Id( *
==+ -
request. 5
.5 6
ParentId6 >
,> ?
cancellationToken@ Q
)Q R
;R S
if 

( 
parent 
is 
null 
) 
return 
Result 
< 
InvitationCodeDto +
>+ ,
., -
Failure- 4
(4 5
$str5 Q
,Q R
$numS V
)V W
;W X
if 

( 
parent 
. 
Role 
!= 
UserRole #
.# $
Parent$ *
)* +
return 
Result 
< 
InvitationCodeDto +
>+ ,
., -
Failure- 4
(4 5
$str5 Y
,Y Z
$num[ ^
)^ _
;_ `
if 

( 
string 
. 
IsNullOrEmpty  
(  !
parent! '
.' (
InvitationCode( 6
)6 7
)7 8
{ 	
parent   
.   $
RegenerateInvitationCode   +
(  + ,
)  , -
;  - .
await!! 
_context!! 
.!! 
SaveChangesAsync!! +
(!!+ ,
cancellationToken!!, =
)!!= >
;!!> ?
}"" 	
return$$ 
Result$$ 
<$$ 
InvitationCodeDto$$ '
>$$' (
.$$( )
Success$$) 0
($$0 1
new%% 
InvitationCodeDto%% !
(%%! "
parent%%" (
.%%( )
Id%%) +
,%%+ ,
parent%%- 3
.%%3 4
InvitationCode%%4 B
!%%B C
)%%C D
)%%D E
;%%E F
}&& 
}'' •
qD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Queries\GetMyParent\GetMyParentQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Queries- 4
.4 5
GetMyParent5 @
;@ A
public 
record 
GetMyParentQuery 
( 
Guid #
	StudentId$ -
)- .
:/ 0
IRequest1 9
<9 :
Result: @
<@ A
ParentInfoDtoA N
?N O
>O P
>P Q
;Q R
public 
record 
ParentInfoDto 
( 
Guid		 
ParentId			 
,		 
string

 

FullName

 
,

 
string 

Email 
) 
; ⁄
xD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Queries\GetMyParent\GetMyParentQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Queries- 4
.4 5
GetMyParent5 @
;@ A
public		 
class		 #
GetMyParentQueryHandler		 $
:		% &
IRequestHandler		' 6
<		6 7
GetMyParentQuery		7 G
,		G H
Result		I O
<		O P
ParentInfoDto		P ]
?		] ^
>		^ _
>		_ `
{

 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
#
GetMyParentQueryHandler "
(" #!
IApplicationDbContext# 8
context9 @
)@ A
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
ParentInfoDto *
?* +
>+ ,
>, -
Handle. 4
(4 5
GetMyParentQuery5 E
requestF M
,M N
CancellationTokenO `
cancellationTokena r
)r s
{ 
var 
student 
= 
await 
_context $
.$ %
Users% *
. 
FirstOrDefaultAsync  
(  !
u! "
=># %
u& '
.' (
Id( *
==+ -
request. 5
.5 6
	StudentId6 ?
,? @
cancellationTokenA R
)R S
;S T
if 

( 
student 
is 
null 
) 
return 
Result 
< 
ParentInfoDto '
?' (
>( )
.) *
Failure* 1
(1 2
$str2 N
,N O
$numP S
)S T
;T U
if 

( 
student 
. 
ParentId 
is 
null  $
)$ %
return 
Result 
< 
ParentInfoDto '
?' (
>( )
.) *
Success* 1
(1 2
null2 6
)6 7
;7 8
var 
parent 
= 
await 
_context #
.# $
Users$ )
. 
FirstOrDefaultAsync  
(  !
u! "
=># %
u& '
.' (
Id( *
==+ -
student. 5
.5 6
ParentId6 >
.> ?
Value? D
&&E G
uH I
.I J
RoleJ N
==O Q
UserRoleR Z
.Z [
Parent[ a
,a b
cancellationTokenc t
)t u
;u v
if   

(   
parent   
is   
null   
)   
return!! 
Result!! 
<!! 
ParentInfoDto!! '
?!!' (
>!!( )
.!!) *
Success!!* 1
(!!1 2
null!!2 6
)!!6 7
;!!7 8
return## 
Result## 
<## 
ParentInfoDto## #
?### $
>##$ %
.##% &
Success##& -
(##- .
new##. 1
ParentInfoDto##2 ?
(##? @
parent$$ 
.$$ 
Id$$ 
,$$ 
parent%% 
.%% 
FullName%% 
,%% 
parent&& 
.&& 
Email&& 
.&& 
Value&& 
)&& 
)&&  
;&&  !
}'' 
}(( ı

{D:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Queries\GetStudentGrades\GetStudentGradesQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Queries- 4
.4 5
GetStudentGrades5 E
;E F
public 
record !
GetStudentGradesQuery #
(# $
Guid$ (
	StudentId) 2
,2 3
Guid4 8
RequesterId9 D
)D E
: 
IRequest 
< 
Result 
< 
List 
< 
StudentGradeDto *
>* +
>+ ,
>, -
;- .
public		 
record		 
StudentGradeDto		 
(		 
Guid

 
Id

	 
,

 
int 
Value 
, 
string 

Description 
, 
DateTime 
GradedAt 
, 
Guid 
ClassroomId	 
, 
string 

ClassroomName 
, 
string 

SubjectName 
, 
Guid 
	TeacherId	 
, 
string 

TeacherName 
) 
; ñ7
ÇD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Queries\GetStudentGrades\GetStudentGradesQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Queries- 4
.4 5
GetStudentGrades5 E
;E F
public		 
class		 (
GetStudentGradesQueryHandler		 )
:

 
IRequestHandler

 
<

 !
GetStudentGradesQuery

 +
,

+ ,
Result

- 3
<

3 4
List

4 8
<

8 9
StudentGradeDto

9 H
>

H I
>

I J
>

J K
{ 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
(
GetStudentGradesQueryHandler '
(' (!
IApplicationDbContext( =
context> E
)E F
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
List !
<! "
StudentGradeDto" 1
>1 2
>2 3
>3 4
Handle5 ;
(; <!
GetStudentGradesQuery< Q
requestR Y
,Y Z
CancellationToken[ l
cancellationTokenm ~
)~ 
{ 
var 
student 
= 
await 
_context $
.$ %
Users% *
. 
FirstOrDefaultAsync  
(  !
u! "
=># %
u& '
.' (
Id( *
==+ -
request. 5
.5 6
	StudentId6 ?
,? @
cancellationTokenA R
)R S
;S T
if 

( 
student 
is 
null 
) 
return 
Result 
< 
List 
< 
StudentGradeDto .
>. /
>/ 0
.0 1
Failure1 8
(8 9
$str9 U
,U V
$numW Z
)Z [
;[ \
var 
isStudentSelf 
= 
request #
.# $
RequesterId$ /
==0 2
request3 :
.: ;
	StudentId; D
;D E
var 
isParent 
= 
student 
. 
ParentId '
.' (
HasValue( 0
&&1 3
student4 ;
.; <
ParentId< D
.D E
ValueE J
==K M
requestN U
.U V
RequesterIdV a
;a b
if 

( 
! 
isStudentSelf 
&& 
! 
isParent '
)' (
return   
Result   
<   
List   
<   
StudentGradeDto   .
>  . /
>  / 0
.  0 1
Failure  1 8
(  8 9
$str  9 d
,  d e
$num  f i
)  i j
;  j k
var"" 
raw"" 
="" 
await"" 
("" 
from## 
g## 
in## 
_context## 
.## 
Grades## %
where$$ 
g$$ 
.$$ 
	StudentId$$ 
==$$  
request$$! (
.$$( )
	StudentId$$) 2
join%% 
c%% 
in%% 
_context%% 
.%% 

Classrooms%% )
on%%* ,
g%%- .
.%%. /
ClassroomId%%/ :
equals%%; A
c%%B C
.%%C D
Id%%D F
join&& 
t&& 
in&& 
_context&& 
.&& 
Users&& $
on&&% '
g&&( )
.&&) *
	TeacherId&&* 3
equals&&4 :
t&&; <
.&&< =
Id&&= ?
orderby'' 
g'' 
.'' 
GradedAt'' 

descending'' )
select(( 
new(( 
{)) 
g** 
.** 
Id** 
,** 
g++ 
.++ 
Value++ 
,++ 
g,, 
.,, 
Description,, 
,,, 
g-- 
.-- 
GradedAt-- 
,-- 
g.. 
... 
ClassroomId.. 
,.. 
ClassroomName// 
=// 
c//  !
.//! "
Name//" &
,//& '
c00 
.00 
SubjectType00 
,00 
g11 
.11 
	TeacherId11 
,11 
TeacherName22 
=22 
t22 
.22  
	FirstName22  )
+22* +
$str22, /
+220 1
t222 3
.223 4
LastName224 <
}33 
)33 
.33 
ToListAsync33 
(33 
cancellationToken33 ,
)33, -
;33- .
var55 
grades55 
=55 
raw55 
.55 
Select55 
(55  
x55  !
=>55" $
new55% (
StudentGradeDto55) 8
(558 9
x66 
.66 
Id66 
,66 
x77 
.77 
Value77 
,77 
x88 
.88 
Description88 
,88 
x99 
.99 
GradedAt99 
,99 
x:: 
.:: 
ClassroomId:: 
,:: 
x;; 
.;; 
ClassroomName;; 
,;; 
SubjectName<< 
(<< 
x<< 
.<< 
SubjectType<< %
)<<% &
,<<& '
x== 
.== 
	TeacherId== 
,== 
x>> 
.>> 
TeacherName>> 
)>> 
)>> 
.>> 
ToList>> "
(>>" #
)>># $
;>>$ %
return@@ 
Result@@ 
<@@ 
List@@ 
<@@ 
StudentGradeDto@@ *
>@@* +
>@@+ ,
.@@, -
Success@@- 4
(@@4 5
grades@@5 ;
)@@; <
;@@< =
}AA 
privateCC 
staticCC 
stringCC 
SubjectNameCC %
(CC% &
SubjectTypeCC& 1
typeCC2 6
)CC6 7
=>CC8 :
typeCC; ?
switchCC@ F
{DD 
SubjectTypeEE 
.EE 
MathematicsEE 
=>EE  "
$strEE# /
,EE/ 0
SubjectTypeFF 
.FF 
RomanianFF 
=>FF 
$strFF  (
,FF( )
SubjectTypeGG 
.GG 
InformaticsGG 
=>GG  "
$strGG# 0
,GG0 1
_HH 	
=>HH
 
$strHH 
}II 
;II 
}JJ ´
qD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Queries\GetUserById\GetUserByIdQuery.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Queries- 4
.4 5
GetUserById5 @
;@ A
public 
record 
GetUserByIdQuery 
( 
Guid #
UserId$ *
)* +
:, -
IRequest. 6
<6 7
Result7 =
<= >
UserDto> E
>E F
>F G
;G H
public		 
record		 
UserDto		 
(		 
Guid

 
Id

	 
,

 
string 

FullName 
, 
string 

Email 
, 
UserRole 
Role 
, 
bool 
IsActive	 
, 
DateTime 
	CreatedAt 
) 
; ¸
xD:\Facultate\.NET\ai-tutor\src\backend\AiTutor.Application\Features\Users\Queries\GetUserById\GetUserByIdQueryHandler.cs
	namespace 	
AiTutor
 
. 
Application 
. 
Features &
.& '
Users' ,
., -
Queries- 4
.4 5
GetUserById5 @
;@ A
public		 
class		 #
GetUserByIdQueryHandler		 $
:		% &
IRequestHandler		' 6
<		6 7
GetUserByIdQuery		7 G
,		G H
Result		I O
<		O P
UserDto		P W
>		W X
>		X Y
{

 
private 
readonly !
IApplicationDbContext *
_context+ 3
;3 4
public 
#
GetUserByIdQueryHandler "
(" #!
IApplicationDbContext# 8
context9 @
)@ A
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
Result 
< 
UserDto $
>$ %
>% &
Handle' -
(- .
GetUserByIdQuery. >
request? F
,F G
CancellationToken 
cancellationToken +
)+ ,
{ 
var 
user 
= 
await 
_context !
.! "
Users" '
. 
FirstOrDefaultAsync  
(  !
u! "
=># %
u& '
.' (
Id( *
==+ -
request. 5
.5 6
UserId6 <
,< =
cancellationToken> O
)O P
;P Q
if 

( 
user 
is 
null 
) 
throw 
new 
NotFoundException '
(' (
nameof( .
(. /
user/ 3
)3 4
,4 5
request6 =
.= >
UserId> D
)D E
;E F
return 
Result 
< 
UserDto 
> 
. 
Success &
(& '
new' *
UserDto+ 2
(2 3
user 
. 
Id 
, 
user 
. 
FullName 
, 
user 
. 
Email 
. 
Value 
, 
user 
. 
Role 
, 
user   
.   
IsActive   
,   
user!! 
.!! 
	CreatedAt!! 
)!! 
)!! 
;!! 
}"" 
}## 