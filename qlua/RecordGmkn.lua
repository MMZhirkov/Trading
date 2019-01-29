
--TQBR   ROSN

p_classcode="TQBR" --Код класса

p_seccode="GMKN" --Код инструмента    


is_run=true

count=1

function main()
      while is_run do
            sleep(10000)
            --message("Hello, World! №"..tostring(count),1)
            count=count+1
      end
end
function OnStop(stop_flag)

      is_run=false
end
function OnQuote(class_code, sec_code)
--message(class_code.."   "..sec_code,1);
		if class_code==p_classcode and sec_code==p_seccode then
  			l_file=io.open("E:\\HistoryQuick\\HistoryGmkn\\01\\28\\HistoryGmkn28012019.txt", "a")
 			tb=getQuoteLevel2(class_code, sec_code)
        tb=getQuoteLevel2(class_code, sec_code)
      local k ="\n".."Time - "..os.date().."\n".."|BID|"
      
            for i=1,tb.bid_count,1 do
                  k=k..(tostring(tb.bid[i].price).." : "..
                        tostring(tb.bid[i].quantity)..";")
            end 
            k=k.."|OFFER|"
            for i=1,tb.offer_count,1 do
                 k=k..(tostring(tb.offer[i].price).." : "..
                        tostring(tb.offer[i].quantity)..";")
            end         
            l_file:write(k)
            l_file:close()
		end
end