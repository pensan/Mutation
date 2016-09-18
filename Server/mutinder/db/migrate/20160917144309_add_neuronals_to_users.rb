class AddNeuronalsToUsers < ActiveRecord::Migration[5.0]

  def change
    add_column :users, :neuronal_network, :json, null: true
  end

end
