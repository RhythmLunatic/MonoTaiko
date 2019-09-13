using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTaiko.Global.Menu
{
    class WheelItem
    {
        //Draw depending on this I guess idk
        public Menu.Categories category { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// This is where you transform the wheel positioning.
        /// </summary>
        /// <param name="item_index"></param>
        /// <param name="num_items"></param>
        /// <param name="is_focus"></param>
        public void Transform(int item_index, int num_items, bool is_focus)
        {
            int offsetFromCenter = item_index - (num_items / 2);
            //self.container:stoptweening();
            if (Math.Abs(offsetFromCenter) < 4)
            {
                //self.container:decelerate(.5);
                //self.container:visible(true);
            }
            else
            {
                //self.container:visible(false);
            }
            //self.container:x(offsetFromCenter * 500);
            //self.container:zoom(math.cos(offsetFromCenter * math.pi / 6) * .8);
        }
        /// <summary>
        /// The MusicWheel will give a MenuItem item and then this object sets it to itself...
        /// There is actually no need to do this...
        /// </summary>
        /// <param name="info"></param>
        /// <param name="info_index"></param>
        /// <param name="item_index"></param>
        public void Set(MenuItem info, int info_index, int item_index)
        {

        }

        /// <summary>
        /// Let the screen's draw function call this so it will be drawn.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {

        }
    }
    /// <summary>
    /// Draws a MusicWheel, handling all the wheel items.
    /// </summary>
    class MusicWheel
    {
        int info_pos;
        int num_items;
        int focus_pos;
        //Unknown purpose
        int mx = 0;
        int my = 0;

        //
        MenuItem[] info_set;
        int[] info_map; //Unknown purpose.
        WheelItem[] wheelItemPool;

        MusicWheel(int num_items, MenuItem[] info_set)
		{
            focus_pos = (num_items / 2);
            //???????
            //item_params= item_params or {}

            //self.info_map= {}
            //check_metatable(item_metatable)
            //self.items= {}
            /*local args= {
				Name= self.name,
				InitCommand= function(subself)
					subself:xy(mx, my)
					self.container= subself
				end
			}
			for n= 1, num_items do
				local item= setmetatable({}, item_metatable)
				local sub_params= DeepCopy(item_params)
				sub_params.name= "item" .. n
				local actor_frame= item:create_actors(sub_params)
				self.items[#self.items+1]= item
				args[#args+1]= actor_frame
			end
			//return Def.ActorFrame(args)*/
            this.info_set = info_set;
            int start_pos = calc_start(1);
            info_pos = start_pos;
			/*for n= 1, #self.items do
				local index= maybe_wrap_index(start_pos, n, info)
				call_item_set(self, n, index)
			end*/
            scroll_wheel(start_pos);
		}

		/*function table.rotate_right(t, r)
			local new_t= {}
			for n= 1, #t do
				local index= ((n - r - 1) % #t) + 1
				new_t[n]= t[index]
			end
			return new_t
		end*/

		public static void LeftShiftArray<T>(T[] arr, int shift)
		{
			shift = shift % arr.Length;
			T[] buffer = new T[shift];
			Array.Copy(arr, buffer, shift);
			Array.Copy(arr, shift, arr, 0, arr.Length - shift);
			Array.Copy(buffer, 0, arr, arr.Length - shift, shift);
		}

		private int wrapped_index(int start, int offset, int set_size)
		{
            return ((start - 1 + offset) % set_size) + 1;
		}

		private int force_to_range(int min, int number, int max)
		{
            return Math.Min(max, Math.Max(min, number));
		}

		private int calc_start(int pos)
		{
            return pos - focus_pos;
		}

		private void call_item_set(int item_index, int info_index)
		{
            info_map[item_index] = info_index;
            //This is the command that sets a new object when you move left or right.
            wheelItemPool[item_index].Set(info_set[info_index], info_index, item_index);
		}

		/*private void no_repeat_internal_scroll(int focus_pos)
		{
			for (int i =0; i < wheelItemPool.Length; i++)
				wheelItemPool[i].Transform(i, wheelItemPool.Length, i == focus_pos, focus_pos, wheelItemPool[i].get_info_at_focus_pos());
		}*/

		private void scroll_wheel(int start_pos)
		{
            int shift_amount = start_pos - info_pos;
			if (Math.Abs(shift_amount) < wheelItemPool.Length)
			{
				LeftShiftArray(wheelItemPool,shift_amount);
				//?????
				LeftShiftArray(info_map,shift_amount);
                info_pos = start_pos;
				if (shift_amount < 0)
				{
                    int absa = Math.Abs(shift_amount);
					for (int n= 1; n<absa+1; n++)
					{
						if (wheelItemPool[n] != null)
						{
                            int info_index = maybe_wrap_index(info_pos, n);
                            call_item_set(n, info_index);
						}
					}
				}
				else if (shift_amount > 0)
				{
					for (int n = wheelItemPool.Length - shift_amount; n < wheelItemPool.Length;)
					{
                        if (wheelItemPool[n] != null)
                        {
                            int info_index = maybe_wrap_index(info_pos, n);
                            call_item_set(n, info_index);
                        }
                    }
				}
			}
			else
			{
                info_pos = start_pos;
				for (int i= 1; i < wheelItemPool.Length; i++)
				{
                    int info_index = maybe_wrap_index(info_pos, i);
                    call_item_set(i, info_index);
				}
			}
			
			for (int i= 1; i < wheelItemPool.Length; i++)
			{
				//int info_index = self:maybe_wrap_index(start_pos, i, self.info_set)
				wheelItemPool[i].Transform(i, wheelItemPool.Length, i == focus_pos);
			}
		}

		/*private void no_repeat_set_info_set(int pos)
		{
			info_pos= 1
			for (int n= 1, n <wheelItemPool.length; n++)
				call_item_set(self, n, n);
			for n= #self.info_set+1, #self.items do
				call_item_set(self, n, n)
			end
			internal_scroll(self, pos)
		}*/

		private int maybe_wrap_index(int ipos, int n)
		{
            return wrapped_index(ipos, n, wheelItemPool.Length);
		}

		/*public void set_element_info(element, info)
		{
			self.info_set[element]= info
			for (int i= 1; i < wheelItemPool.length; i++)
			{
				if (self.info_map[i] == element) then
					call_item_set(self, i, element)
				end
			}
		}*/
		/*I don't know what scroll_to_pos is even supposed to do but it's pretty much useless
		Whatever it does, it doesn't use the info index*/
		public void scroll_to_pos(int pos)
		{
            int start_pos = calc_start(pos);
            scroll_wheel(start_pos);
		}
		/*it scrolls by the info_pos minus the current index.
		So if the index is negative, it scrolls to the start... somehow.*/
		public void scroll_to_start()
		{
            scroll_wheel(
                info_pos - maybe_wrap_index(info_pos, focus_pos) + 1);
		}
		/*This is a lie, since we don't want to ACTUALLY scroll to the end on the DDR A wheel,
		just the last row.*/
		public void scroll_to_end()
		{
			scroll_by_amount((info_set.Length - get_current_index())/3*3);
		}
		public void scroll_by_amount(int amount)
		{
            scroll_wheel(info_pos + amount);
		}
		public MenuItem get_info_at_focus_pos()
		{
            int index = maybe_wrap_index(
                        info_pos,
                        focus_pos
                    );
			return info_set[index];
		}
		public MenuItem get_info_at_offset_from_current(int offset)
		{
            //TODO: Should use a pointer instead
            int index = get_current_index();
			/*
				ex. Index is 55. Size of array is 56. We want element 57 (offset of 2), which should be 1.
				return element 2-(56-55) = 1
				return element OFFSET-(SIZE-INDEX)
			*/
			if (index+offset > info_set.Length)
				return info_set[offset-(info_set.Length-index)];
			/*
				ex. Index is 5. Size of array is 50. We want element that is 10 less than current, which should be 45.
				return SIZE-OFFSET+INDEX
				return 50-10+5 = 45
			*/
			else if (index+offset < 0)
				return info_set[info_set.Length-offset+index];
			else
				return info_set[index+offset];
		}
		public WheelItem get_actor_item_at_focus_pos()
		{
			return wheelItemPool[focus_pos];
		}
		
		public List<WheelItem> get_items_by_info_index(int index)
		{
			List<WheelItem> ret = new List<WheelItem>();
			for (int i= 1; i < wheelItemPool.Length; i++)
			{
				if (info_map[i] == index)
					ret.Add(wheelItemPool[i]);
			}
			return ret;
		}
		
		/*find_item_by_info= function(self, info)
			local ret= {}
			for i, item in ipairs(self.items) do
				if item.info == info then
					ret[#ret+1]= item
				end
			end
			return ret
		end,*/
		public void move_focus_by(int pos)
		{
            focus_pos = focus_pos + pos;
            scroll_wheel(info_pos);
		}
		/*--WARN: Might need to optimize
		--If below the middle (to the left) return 0, if above (to the right) return 2, if at the middle return 1
		get_ddra_focus=function(self,pos)
			if self.focus_pos < math.floor(self.num_items / 2)+1 then
				return 0
			elseif self.focus_pos > math.floor(self.num_items / 2)+1 then
				return 2
			else
				return 1
			end;
		end;*/
		/*public int get_wheel_item_type=function(self)
			return self:get_info_at_focus_pos()[1]
		end;*/
		//Gets the current index, duh
		public int get_current_index()
		{
            return maybe_wrap_index(info_pos, focus_pos);
		}
    }

}
