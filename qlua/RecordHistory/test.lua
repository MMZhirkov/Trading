--TQBR   RASP
--TQBR   VTBR
--TQBR   GMKN
--TQBR   ROSN

p_classcode="TQBR" --Код класса

p_seccode="RASP" --Код инструмента    


is_run=true

count=1

function main()
      -- Создает таблицу
   CreateTable();
   
   -- Основной цикл
   while is_run do
      sleep(10000);
      
   end;
end
function CreateTable()
   -- Получает доступный id для создания
   t_id = AllocTable(); 
   -- Добавляет 5 колонок
   AddColumn(t_id, 0, "Active", true, QTABLE_INT_TYPE, 15);
   AddColumn(t_id, 1, "Qte", true, QTABLE_INT_TYPE, 15);
   AddColumn(t_id, 2, "Price", true, QTABLE_INT_TYPE, 15);
   AddColumn(t_id, 3, "Side", true, QTABLE_INT_TYPE, 15);
   AddColumn(t_id, 4, "Time", true, QTABLE_INT_TYPE, 15);
   -- Создаем
   t = CreateWindow(t_id);
   -- Даем заголовок 
   SetWindowCaption(t_id, "Clusters");
   -- Добавляет строку
   InsertRow(t_id, -1);
  SetCell(t_id, 1, 1, tostring("first"));
  SetCell(t_id, 1, 2, tostring("second"));
   InsertRow(t_id, -1);
    SetCell(t_id, 2, 1, tostring("first"));
  SetCell(t_id, 2, 2, tostring("second"));
end;
function addCluster(t_id,)

end;
function OnQuote(class_code, sec_code)
  if class_code==p_classcode and sec_code==p_seccode then
    tb=getQuoteLevel2(class_code, sec_code)
    
    
  end;
end;
function OnStop(stop_flag)

      is_run=false
end
--function OnQuote(class_code, sec_code)
--message(class_code.."   "..sec_code,1);
--end