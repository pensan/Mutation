class CreateUsers < ActiveRecord::Migration[5.0]

  def change
    create_table :users do |t|
      t.string :username, null: false, default: ''
      t.string :uuid, null: false, default: ''

      t.timestamps
    end

    add_index :users, :username
    add_index :users, :uuid, unique: true
  end

end
