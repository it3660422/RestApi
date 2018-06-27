use master;
go

create table CREDIT_CARD (
	CARD_ID int not null identity(1,1) primary key,
	CARD_NO int not null
);
go

insert into CREDIT_CARD(CARD_NO)
select 4111111111111111 union all
select 5111111111111111 union all
select 311111111111111 union all
select 3111111111111111;
go

if OBJECT_ID ( 'checkCard') is not null   
    drop procedure checkCard;  
go
create procedure checkCard
    @cardNo bigint
as
begin
    select count(*) as CNT from CREDIT_CARD where CARD_NO=@cardNo;
end;
go